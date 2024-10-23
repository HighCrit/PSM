using Antlr4.Runtime.Tree;
using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;

namespace PSM.Parsers.Labels;

public class TransitionLabelVisitor : TransitionLabelsBaseVisitor<IExpression>
{
    private const string @bool = "Bool";
    private const string @int = "Int";

    private Dictionary<string, ModelInfo>? atlas;

    public IExpression Visit(Dictionary<string, ModelInfo> a, IParseTree pt)
    {
        try
        {
            this.atlas = a;
            return this.Visit(pt);
        }
        finally
        {
            this.atlas = null;
        }
    }

    public override IExpression VisitLabel(TransitionLabelsParser.LabelContext context)
    {
        return this.Visit(context.orExpr());
    }

    public override IExpression VisitOrExpr(TransitionLabelsParser.OrExprContext context)
    {
        var exps = context.andExpr().Select(this.Visit).ToArray();
        return new Or(exps);
    }
    
    public override IExpression VisitAndExpr(TransitionLabelsParser.AndExprContext context)
    {
        var exps = context.negExpr().Select(this.Visit).ToArray();
        return new And(exps);
    }
    
    public override IExpression VisitNegExpr(TransitionLabelsParser.NegExprContext context)
    {
        var v = context.val();
        var neg = context.negExpr();

        if (v is not null) return this.Visit(v);
        if (neg is not null) return this.Visit(neg);
        throw new ArgumentNullException();
    }

    public override IExpression VisitVariable(TransitionLabelsParser.VariableContext context)
    {
        var lhs = context.PATH().GetText()!;
        var rhs = context.variable_val()?.PATH().GetText();

        if (!this.atlas!.TryGetValue(lhs, out var lhsInfo) || lhsInfo.Type is not ModelPropertyType.Variable)
        {
            throw new ArgumentException($"Unknown variable '{lhs}'");
        }

        ModelInfo? rhsInfo = null;
        if (rhs is not null && (!this.atlas!.TryGetValue(rhs, out rhsInfo) || rhsInfo.Type is not ModelPropertyType.Variable))
        {
            throw new ArgumentException($"Unknown variable '{rhs}'");
        }

        if (rhs is null && string.IsNullOrWhiteSpace(context.variable_val()?.GetText()))
        {
            // booleanValue
            return new Variable(
                lhsInfo,
                "==",
                @bool,
                "true");
        }
        else if (rhs is null)
        {
            // variable OP value
            return new Variable(
                lhsInfo,
                context.VARIABLE_OP().GetText(),
                this.GetDomain(context),
                context.variable_val().GetText());
        }
        else if (rhsInfo is not null)
        {
            if (lhsInfo.ValueType != rhsInfo.ValueType)
            {
                throw new ArgumentException($"Cannot compare type '{lhsInfo.ValueType}' with '{rhsInfo.ValueType}'.");
            }

            // variable OP variable
            return new Variable(
                lhsInfo,
                context.VARIABLE_OP().GetText(),
                lhsInfo.ValueType == "Boolean" ? @bool : @int,
                rhsInfo);
        }

        throw new NotSupportedException();
    }

    public override IExpression VisitCommand(TransitionLabelsParser.CommandContext context)
    {
        var commandPath = context.PATH().GetText();
        if (!this.atlas!.TryGetValue(commandPath, out var info) || info.Type is not ModelPropertyType.Command)
        {
            throw new ArgumentException($"Unknown command '{commandPath}'");
        }

        return new Command(info.MachineIndex, info.Name);
    }

    public override IExpression VisitVal(TransitionLabelsParser.ValContext context)
    {
        var command = context.command();
        var variable = context.variable();
        var or = context.orExpr();

        if (command is not null) return this.Visit(command);
        if (variable is not null) return this.Visit(variable);
        if (or is not null) return this.Visit(or);
        throw new ArgumentNullException();
    }

    private string GetDomain(TransitionLabelsParser.VariableContext context)
    {
        if (context.VARIABLE_OP().GetText() is "<" or ">") return @int;
        
        var val = context.variable_val();
        return val?.BOOLEAN() is not null ? @bool : @int;
    }
}