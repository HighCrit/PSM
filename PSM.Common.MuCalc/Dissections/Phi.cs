// <copyright file="Phi.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.ActionFormula;
using PSM.Common.MuCalc.ActionFormula.Operators;
using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Common.Operators;
using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.Dissections.Labels.Operations;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.ModalFormula.Operators;
using PSM.Common.MuCalc.RegularFormula;
using PSM.Common.MuCalc.RegularFormula.Operators;
using Action = PSM.Common.MuCalc.Actions.Action;

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

    public IModalFormula ToMuCalc(Dictionary<Event, IExpression>? substitutions)
    {
        var expressions = this.GetLabelExpressions(substitutions);

        // TODO: Add support for Fix and Neg
        var expression = expressions[0];

        return this.ParseExpression(expression);
    }

    public IModalFormula ParseExpression(IExpression expression, IActionFormula? commands = null)
    {
        return expression switch
        {
            And and => this.ParseAnd(and, commands),
            Or or => this.ParseOr(or, commands),
            Variable v => this.ParseVariable(v, commands),
            Command c => this.ParseCommand(c),
            _ => throw new ArgumentException(nameof(expression))
        };
    }

    public IModalFormula ParseAnd(And and, IActionFormula? commands)
    {
        // All sub nodes are connected by And, therefore we can simplify
        if (!and.ContainsOr())
        {
            var variables = this.GetConjunctionOfVariables(and)!;
            var com = this.GetUnionOfCommands(and);

            return this.GetVarForType(variables, com, this.Sigma);
        }

        if (!and.Left.ContainsVariables())
        {
            var com = this.GetUnionOfCommands(and.Left);
            return this.ParseExpression(and.Right, com);
        }
        else if (!and.Right.ContainsVariables())
        {
            var com = this.GetUnionOfCommands(and.Right);
            return this.ParseExpression(and.Left, com);
        }

        return new Conjunction(this.ParseExpression(and.Left), this.ParseExpression(and.Right));
    }

    public IModalFormula ParseOr(Or or, IActionFormula? commands)
    {
        // All sub nodes are connected by Or, therefore we can simplify
        if (!or.ContainsAnd())
        {
            var variables = this.GetDisjunctionOfVariables(or);
            var com = this.GetUnionOfCommands(or);

            IModalFormula? varPart = null;
            if (variables is not null)
            {
                // commands is only non-null if this side of AND is variable
                varPart = this.GetVarForType(variables, commands, this.Sigma);
            }

            IModalFormula? comPart = null;
            if (com is not null)
            {
                comPart = this.GetComBoxForType(com, this.Sigma);
            }

            if (varPart is null)
            {
                return comPart!;
            }
            else if (comPart is null)
            {
                return varPart!;
            }

            return new Disjunction(varPart, comPart);
        }

        return new Disjunction(this.ParseExpression(or.Left), this.ParseExpression(or.Right));
    }

    public IModalFormula ParseCommand(Command? command)
    {
        IActionFormula af;
        if (command is not null)
        {
            af = this.SingleCom(command);
            if (type is PhiType.Neg or PhiType.Fix)
            {
                af = new Complement(af);
            }
        }
        else
        {
            af = new ActionFormula.ActionFormula(Action.True);
        }

        return this.Type switch
        {
            PhiType.Pos => new Box(new RegularFormula.ActionFormula(af), this.Sigma),
            PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(af)), this.Sigma),
            PhiType.Fix => new Box(new RegularFormula.ActionFormula(af), this.Sigma),
            _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
        };
    }

    public IModalFormula ParseVariable(Variable variable, IActionFormula? commands) =>
        this.GetVarForType(this.SingleVar(variable), commands, this.Sigma);

    private IModalFormula GetComBoxForType(IActionFormula af, IModalFormula sigma)
    {
        // Only negate the af if type is neg or fix and it is not true.
        if (this.Type is PhiType.Neg or PhiType.Fix && !af.Equals(new ActionFormula.ActionFormula(Action.True)))
        {
            af = new Complement(af);
        }

        return this.Type switch
        {
            PhiType.Pos => new Box(new RegularFormula.ActionFormula(af), sigma),
            PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(af)), sigma),
            PhiType.Fix => new Box(new RegularFormula.ActionFormula(af), sigma),
            _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
        };
    }

    private IModalFormula GetVarForType(IModalFormula variables, IActionFormula? commands, IModalFormula sigma)
    {
        return this.Type switch
        {
            PhiType.Pos => new Implication(
                variables,
                this.GetComBoxForType(commands ?? new ActionFormula.ActionFormula(Action.True), sigma)),
            PhiType.Neg => this.GetComBoxForType(
                commands ?? new ActionFormula.ActionFormula(Action.True),
                new Implication(variables, sigma)),
            PhiType.Fix => this.GetComBoxForType(
                commands ?? new ActionFormula.ActionFormula(Action.True),
                new Implication(new Negation(variables), sigma)),
            _ => throw new ArgumentException()
        };
    }

    private IModalFormula? GetConjunctionOfVariables(IExpression expression)
    {
        using var variables = expression.GetVariablesInSubTree().GetEnumerator();
        if (!variables.MoveNext())
        {
            return null;
        }

        IModalFormula formula = this.SingleVar(variables.Current);
        while (variables.MoveNext())
        {
            formula = new Conjunction(
                formula,
                this.SingleVar(variables.Current));
        }

        return formula;
    }

    private IModalFormula? GetDisjunctionOfVariables(IExpression expression)
    {
        using var variables = expression.GetVariablesInSubTree().GetEnumerator();
        if (!variables.MoveNext())
        {
            return null;
        }

        IModalFormula formula = this.SingleVar(variables.Current);
        while (variables.MoveNext())
        {
            formula = new Disjunction(
                formula,
                this.SingleVar(variables.Current));
        }

        return formula;
    }

    private IActionFormula? GetUnionOfCommands(IExpression expression)
    {
        using var commands = expression.GetCommandsInSubTree().GetEnumerator();
        if (!commands.MoveNext())
        {
            return null;
        }

        IActionFormula formula = new ActionFormula.ActionFormula(new Action("command", [commands.Current.Name, "true"]));
        while (commands.MoveNext())
        {
            formula = new Union(
                formula,
                new ActionFormula.ActionFormula(new Action("command", [commands.Current.Name, "true"])));
        }

        return formula;
    }

    private IModalFormula SingleVar(Variable var)
    {
        return new Exists<IModalFormula>(
            "s_1",
            var.Domain,
            new Conjunction(
                new Diamond(
                    new RegularFormula.ActionFormula(
                        new ActionFormula.ActionFormula(new Action(var.Name, ["s_1"]))),
                    Bool.True),
                new BooleanExp($"s_1 {var.Operand} {var.Value}")));
    }

    private IActionFormula SingleCom(Command com)
    {
        return new ActionFormula.ActionFormula(new Action("command", [com.Name, "true"]));
    }

    public string ToLatex(Dictionary<Event, IExpression> substitutions) =>
        this.ToMuCalc(substitutions).ToLatex(substitutions);

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions) =>
        this.ToMuCalc(substitutions).ToMCRL2(substitutions);
}
