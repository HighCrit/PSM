using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;

namespace PSM.Parsers.Labels;

public class TransitionLabelVisitor : TransitionLabelsBaseVisitor<IExpression>
{
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
        return new Variable(
            context.IDENTIFIER().GetText(),
            context.VARIABLE_OP().GetText(),
            this.GetDomain(context), 
            context.variable_val().GetText());
    }

    public override IExpression VisitCommand(TransitionLabelsParser.CommandContext context)
    {
        return new Command(context.IDENTIFIER().GetText());
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
        const string @bool = "Bool";
        const string @int = "Int";
        if (context.VARIABLE_OP().GetText() is "<" or ">") return @int;
        
        var val = context.variable_val();
        return val?.BOOLEAN() is not null ? @bool : @int;
    }
}