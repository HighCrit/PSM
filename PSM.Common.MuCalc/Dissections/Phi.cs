// <copyright file="Phi.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Diagnostics;
using PSM.Common.MuCalc.ActionFormula;
using PSM.Common.MuCalc.ActionFormula.Operators;
using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Common.Operators;
using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.Dissections.Labels.Operations;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.ModalFormula.Operators;
using Action = PSM.Common.MuCalc.Actions.Action;
using ArgumentException = System.ArgumentException;

namespace PSM.Common.MuCalc.Dissections;

public class Phi(PhiType type, Event ev, IModalFormula sig) : IModalFormula
{
    public PhiType Type { get; } = type;
    public Event Ev { get; } = ev;
    public IModalFormula Sigma { get; } = sig;

    public List<IExpression> GetLabelExpressions(Dictionary<Event, IExpression>? substitutions)
    {
        return this.Ev.GetFlags().Select(
            e =>
            {
                if (substitutions is null)
                {
                    // Only used by ToString
                    return new Command(e.ToString());
                }
                if (!substitutions.TryGetValue(e, out var label))
                {
                    throw new ArgumentException($"Missing expected event '{e}' value.");
                }

                return label;
            }).ToList();
    }

    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        var sigma = this.Sigma.ApplySubstitutions(substitutions);
        var expressions = this.GetLabelExpressions(substitutions)
            .Select(e => e.ToDNF()).ToList();

        if (expressions.Count == 1)
        {
            return this.ParseExpression(sigma, expressions[0]);
        }
        // Sometimes used by Neg and Fix patterns
        if (expressions.Count == 2 && this.Type is PhiType.Neg or PhiType.Fix)
        {
            var firstExp = expressions[0];
            var secondExp = expressions[1];

            if (firstExp is Or || secondExp is Or)
            {
                throw new NotImplementedException();
            }
            else
            {
                return this.ParseDoubleExpressions(sigma, firstExp, secondExp);
            }
        }

        throw new ArgumentException($"Expected 1 or 2 expressions, got {expressions.Count}");
    }

    public IModalFormula ParseExpression(IModalFormula sigma, IExpression expression, IList<Command>? commands = null)
    {
        var res = expression switch
        {
            And and => this.ParseAnd(sigma, and, commands),
            Or or => this.ParseOr(sigma, or),
            Variable v => this.ParseVariable(sigma, v),
            Command c => this.ParseCommand(sigma, c),
            _ => throw new ArgumentException(null, nameof(expression))
        };

        return res;
    }

    public IModalFormula ParseDoubleExpressions(IModalFormula sigma, IExpression exp1, IExpression exp2)
    {
        Debug.Assert(exp1 is Variable or Command or And);
        Debug.Assert(exp2 is Variable or Command or And);

        var allCommands = exp1.GetCommandsInSubTree()
            .Concat(exp2.GetCommandsInSubTree())
            .ToList();
        var commandsAf = this.GetUnionOfCommands(allCommands);

        var variables = exp1.GetVariablesInSubTree()
            .Concat(exp2.GetVariablesInSubTree())
            .ToList();
        var variablesMf = this.GetConjunctionOfExpressions(sigma, variables);

        return this.GetPhiPattern(sigma, variables: variablesMf, commands: commandsAf);
    }

    private IModalFormula ParseExpressionInt(IModalFormula sigma, IExpression expression, IList<Command>? commands = null)
    {
        return expression switch
        {
            And and => this.ParseAnd(sigma, and, commands),
            Or or => this.ParseOr(sigma, or),
            Variable v => this.SingleVar(v),
            Command c => this.ParseCommand(sigma, c),
            _ => throw new ArgumentException(null, nameof(expression))
        };
    }

    public IModalFormula ParseAnd(IModalFormula sigma, And and, IList<Command>? allCommands)
    {
        allCommands ??= [];

        var commandAf = this.GetDefaultCommand();
        Command? thisCommand = and.GetCommandsInSubTree().SingleOrDefault();

        if (allCommands.Any() || thisCommand is not null)
        {
            var otherCommands = allCommands.Where(c => !c.Equals(thisCommand)).ToList();
            var thisCommandAf = thisCommand is null ? commandAf : this.SingleCom(thisCommand);

            commandAf = this.Type switch
            {
                PhiType.Pos => thisCommandAf,
                PhiType.Neg or PhiType.Fix => otherCommands.Aggregate(
                    thisCommandAf,
                    (af, c) => new Union(af, this.SingleCom(c))),
                _ => throw new ArgumentException("Illegal type provided.")
            };
        }

        var variableMf = and.Expressions.OfType<Variable>().Aggregate(
            (IModalFormula)Bool.True,
            (mf, v) => new Conjunction(mf, this.ParseExpressionInt(sigma, v)));

        return this.GetPhiPattern(sigma, variables: variableMf, commands: commandAf);
    }


    public IModalFormula ParseOr(IModalFormula sigma, Or or)
    {
        var commands = or.Expressions.OfType<Command>().ToList();
        var variables = or.Expressions.Where(e => e is Variable).ToList();
        var subCommands = or.GetCommandsInSubTree().ToList();

        IModalFormula commandMf = Bool.True;
        if (commands.Any())
        {
            var commandAf = commands.Aggregate(
                (IActionFormula)new ActionFormula.ActionFormula(Action.False),
                (af, c) => new Union(af, this.SingleCom(c)));

            commandMf = this.GetPhiPattern(sigma, commands: commandAf);
        }

        IModalFormula variablesMf = Bool.True;
        if (variables.Any())
        {
            var variablesPhiMf = variables.Aggregate(
                (IModalFormula)Bool.False,
                (mf, v) => new Disjunction(mf, this.ParseExpressionInt(sigma, v)));

            variablesMf = this.GetPhiPattern(sigma, variables: variablesPhiMf);
        }

        var remainingExp = or.Expressions.Where(e => !commands.Contains(e) && !variables.Contains(e)).ToList();
        IModalFormula remainingMf = Bool.True;
        if (remainingExp.Any())
        { 
            remainingMf = remainingExp.Aggregate(
                (IModalFormula)Bool.True,
                (mf, exp) => new Conjunction(mf, this.ParseExpression(sigma, exp, subCommands)));
        }

        return new Conjunction(
            new Conjunction(
                variablesMf,
                commandMf),
            remainingMf);
    }

    public IModalFormula ParseCommand(IModalFormula sigma, Command command)
    {
        var af = this.SingleCom(command);

        return this.GetPhiPattern(sigma, commands: af);
    }

    public IModalFormula ParseVariable(IModalFormula sigma, Variable variable) =>
        this.GetPhiPattern(sigma, variables: this.SingleVar(variable));


    private IModalFormula GetPhiPattern(IModalFormula sigma, IModalFormula? variables = null, IActionFormula? commands = null)
    {
        variables ??= this.GetDefaultVariable();
        commands ??= this.GetDefaultCommand();

        return this.Type switch
        {
            PhiType.Pos => new Implication(
                variables,
                this.GetComBoxForType(commands, sigma)),
            PhiType.Neg => new NuFixPoint(
                "P_1",
                new Conjunction(
                    new Implication(
                        new Negation(variables), // If
                        this.GetComBoxForType(commands, new FixPoint("P_1"))),
                    sigma)),
            PhiType.Fix => new Implication(
                    new Negation(variables), // If
                    this.GetComBoxForType(commands, sigma)),
            _ => throw new ArgumentException()
        };
    }

    private IActionFormula GetUnionOfCommands(IEnumerable<Command> commands)
    {
        var enumerable = commands as Command[] ?? commands.ToArray();
        if (!enumerable.Any())
        {
            return this.GetDefaultCommand();
        }

        return enumerable.Aggregate(
            (IActionFormula)new ActionFormula.ActionFormula(Action.False),
            (af, c) => new Union(af, this.SingleCom(c)));
    }
    private IModalFormula GetConjunctionOfExpressions(IModalFormula sigma, IEnumerable<IExpression> exps)
    {
        var enumerable = exps as IExpression[] ?? exps.ToArray();
        if (!enumerable.Any())
        {
            return this.GetDefaultVariable();
        }

        return enumerable.Aggregate(
            (IModalFormula)Bool.True,
            (mf, c) => new Conjunction(mf, this.ParseExpressionInt(sigma, c)));
    }

    private IModalFormula GetComBoxForType(IActionFormula commands, IModalFormula sigma)
    {
        return this.Type switch
        {
            PhiType.Pos => new Box(new RegularFormula.ActionFormula(commands), sigma),
            PhiType.Neg or PhiType.Fix => new Box(new RegularFormula.ActionFormula(new Complement(commands)), sigma),
            _ => throw new ArgumentException()
        };
    }

    private IModalFormula SingleVar(Variable var)
    {
        var res = new Diamond(
            new RegularFormula.ActionFormula(
                new ActionFormula.ActionFormula(new Action(var.Name, [var.Value]))),
            Bool.True);

        return var.Negated ? new Negation(res) : res;
        /*return new Exists<IModalFormula>(
            "s_1",
            var.Domain,
            new Conjunction(
                new Diamond(
                    new RegularFormula.ActionFormula(
                        new ActionFormula.ActionFormula(new Action(var.Name, ["s_1"]))),
                    Bool.True),
                new BooleanExp($"s_1 {var.Operand} {var.Value}")));*/
    }

    private IActionFormula SingleCom(Command com)
    {
        return new ActionFormula.ActionFormula(new Action("command", [com.Name, "true"]));
    }

    private IModalFormula GetDefaultVariable()
    {
        return this.Type switch
        {
            PhiType.Pos => Bool.True,
            PhiType.Neg or PhiType.Fix => Bool.False,
            _ => throw new ArgumentException("Illegal type provided.")
        };
    }

    private IActionFormula GetDefaultCommand()
    {
        return this.Type switch
        {
            PhiType.Pos => new ActionFormula.ActionFormula(Action.True),
            PhiType.Neg or PhiType.Fix => new ActionFormula.ActionFormula(Action.False),
            _ => throw new ArgumentException("Illegal type provided.")
        };
    }

    public IModalFormula Flatten() => this;

    public string ToLatex() => $@"\phi_{this.Type}_{this.Sigma.ToLatex()}";

    public string ToMCRL2() => $@"phi_{this.Type}_{this.Sigma.ToMCRL2()}";
}
