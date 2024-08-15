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

public class Phi(PhiType type, Event ev, IModalFormula sigma) : IModalFormula
{
    public PhiType Type { get; } = type;
    public Event Ev { get; } = ev;
    public IModalFormula Sigma { get; } = sigma;

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

    public IModalFormula ParseExpression(IExpression expression)
    {
        IModalFormula variables = Bool.True;
        IActionFormula commands = new ActionFormula.ActionFormula(Action.True);

        if (expression is Or or)
        {
            if (or.ContainsAnd())
            {
                return new Disjunction(this.ParseExpression(or.Left), this.ParseExpression(or.Right));
            }

            // All further nodes are or'ed, thus we can simplify
            commands = this.GetUnionOfCommands(or);
            variables = this.GetDisjunctionOfVariables(or);

            return this.Type switch
            {
                PhiType.Pos => new Disjunction(new Implication(variables, new Box(Bool.True, this.Sigma)), new Box(new RegularFormula.ActionFormula(commands), this.Sigma)),
                PhiType.Neg => new Disjunction(new Box(new Kleene(Bool.True), new Implication(variables, this.Sigma)), new Box(new Kleene(new RegularFormula.ActionFormula(new Complement(commands))), this.Sigma)),
                PhiType.Fix => new Disjunction(new Box(Bool.True, new Implication(new Negation(variables), this.Sigma)), new Box(new RegularFormula.ActionFormula(new Complement(commands)), this.Sigma)),
                _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
            };
        }
        else if (expression is And and)
        {
            // All further nodes are and'ed, thus we can simplify
            if (!and.ContainsOr())
            {
                commands = this.GetUnionOfCommands(and); // Can only be 1 since it is And
                variables = this.GetConjunctionOfVariables(and);
            }
            // We can optimize this as the variables exist at the same level
            else if (and.Left is Variable leftVar && and.Right is Variable rightVar)
            {
                variables = new Conjunction(this.SingleVar(leftVar), this.SingleVar(rightVar));
            }
            else if (and.Left is Command commandLeft)
            {
                commands = new ActionFormula.ActionFormula(new Action("command", [commandLeft.Name, "true"]));
                variables = this.ParseExpression(and.Right);
            }
            else if (and.Right is Command commandRight)
            {
                commands = new ActionFormula.ActionFormula(new Action("command", [commandRight.Name, "true"]));
                variables = this.ParseExpression(and.Left);
            }
            else
            {
                variables = new Implication(this.ParseExpression(and.Left), this.ParseExpression(and.Right));
            }
        }
        else if (expression is Command command)
        {
            commands = new ActionFormula.ActionFormula(new Action("command", [command.Name, "true"]));
        }
        else if (expression is Variable variable)
        {
            variables = new Exists<IModalFormula>(
                "s_1",
                variable.Domain,
                new Conjunction(
                    new Diamond(
                        new RegularFormula.ActionFormula(
                            new ActionFormula.ActionFormula(new Action($"{variable.Name}(s_1)"))),
                        Bool.True),
                    new BooleanExp($"s_1 {variable.Operand} {variable.Value}")));
        }

        if (this.Type is PhiType.Neg or PhiType.Fix && !commands.Equals(new ActionFormula.ActionFormula(Action.True)))
        {
            commands = new Complement(commands);
        } 

        return this.Type switch
        {
            PhiType.Pos => new Implication(variables, new Box(new RegularFormula.ActionFormula(commands), this.Sigma)),
            PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(commands)), new Implication(variables, this.Sigma)),
            PhiType.Fix => new Box(new RegularFormula.ActionFormula(commands), new Implication(new Negation(variables), this.Sigma)),
            _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
        };

        /*if (expression is And and)
        {
            if (and.Left is Command || and.Left.ContainsCommands())
            {
                var commands = this.ParseExpression(and.Left);
                var variables = and.Right.ContainsOr() ? this.ParseExpression(and.Right) : this.GetConjunctionOfVariables(and.Right);

                switch (this.Type)
                {
                    case PhiType.Pos:
                        break;
                    case PhiType.Neg:
                        break;
                    case PhiType.Fix:
                        break;
                }
            }
            if (and.Right is Command || and.Right.ContainsCommands())
            {
                var commands = this.ParseExpression(and.Right);
                var variables = and.Right.ContainsOr() ? this.ParseExpression(and.Left) : this.GetConjunctionOfVariables(and.Left);

                return new Implication(variables, commands);
            }
            if (and.Left is Variable leftVar && and.Right is Variable rightVar)
            {
                return new Implication(
                    new Conjunction(this.SingleVar(leftVar), this.SingleVar(rightVar)),
                    new Box(Bool.True, this.Sigma));
            }
            return new Conjunction(this.ParseExpression(and.Left), this.ParseExpression(and.Right));
        }
        if (expression is Or or)
        {
            if (or.ContainsAnd() || or.ContainsVariables())
            {
                return new Disjunction(this.ParseExpression(or.Left), this.ParseExpression(or.Right));
            }

            // If largest common OR on exclusively commands then yield the union between all commands.
            var unionOfCommands = this.GetUnionOfCommands(or);

            return this.Type switch
            {
                PhiType.Pos => new Box(new RegularFormula.ActionFormula(unionOfCommands), this.Sigma),
                PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(new Complement(unionOfCommands))), this.Sigma),
                PhiType.Fix => new Box(new RegularFormula.ActionFormula(new Complement(unionOfCommands)), this.Sigma),
                _ => throw new ArgumentException(),
            };
        }
        if (expression is Command command)
        {
            IActionFormula com = new ActionFormula.ActionFormula(new Action("command", [command.Name, "true"]));

            return this.Type switch
            {
                PhiType.Pos => new Box(new RegularFormula.ActionFormula(com), this.Sigma),
                PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(new Complement(com))), this.Sigma),
                PhiType.Fix => new Box(new RegularFormula.ActionFormula(new Complement(com)), this.Sigma),
                _ => throw new ArgumentException(),
            };
        }
        if (expression is Variable variable)
        {
            return new Implication(this.SingleVar(variable), new Box(Bool.True, this.Sigma));
        }*/

        throw new ArgumentException("Failed to parse expression.");
    }

    private IModalFormula GetConjunctionOfVariables(IExpression expression)
    {
        using var variables = expression.GetVariablesInSubTree().GetEnumerator();
        variables.MoveNext();

        IModalFormula formula = this.SingleVar(variables.Current);
        while (variables.MoveNext())
        {
            formula = new Conjunction(
                formula,
                this.SingleVar(variables.Current));
        }

        return formula;
    }

    private IModalFormula GetDisjunctionOfVariables(IExpression expression)
    {
        using var variables = expression.GetVariablesInSubTree().GetEnumerator();
        variables.MoveNext();

        IModalFormula formula = this.SingleVar(variables.Current);
        while (variables.MoveNext())
        {
            formula = new Disjunction(
                formula,
                this.SingleVar(variables.Current));
        }

        return formula;
    }

    private IActionFormula GetUnionOfCommands(IExpression expression)
    {
        using var commands = expression.GetCommandsInSubTree().GetEnumerator();
        commands.MoveNext();

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

    public string ToLatex(Dictionary<Event, IExpression> substitutions) =>
        this.ToMuCalc(substitutions).ToLatex(substitutions);

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions) =>
        this.ToMuCalc(substitutions).ToMCRL2(substitutions);
}
