// <copyright file="Phi.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Diagnostics;
using PSM.Common.MuCalc.ActionFormula;
using PSM.Common.MuCalc.ActionFormula.Operators;
using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Common.Operators;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.ModalFormula.Operators;
using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;
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
                    return new Command(0, e.ToString());
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
            .Select(this.ParseExpression).ToList();
        
        if (expressions.Count == 1)
        {
            return this.GetPhiPattern(expressions[0], sigma);
        }
        // Sometimes used by Neg and Fix patterns
        if (expressions.Count == 2 && this.Type is PhiType.Neg or PhiType.Fix)
        {
            return this.GetPhiPattern(new Disjunction(expressions[0], expressions[1]), sigma);
        }

        throw new ArgumentException($"Expected 1 or 2 expressions, got {expressions.Count}");
    }

    public IModalFormula ParseExpression(IExpression exp)
    {
        return exp switch
        {
            And a => this.ParseAnd(a),
            Or o => this.ParseOr(o),
            Neg n => this.ParseNeg(n),
            Command c => this.ParseCommand(c),
            Variable v => this.ParseVariable(v),
            _ => throw new ArgumentException()
        };
    }

    public IModalFormula ParseAnd(And and)
    {
        return and.Expressions.Aggregate<IExpression, IModalFormula>(
            Bool.True,
            (mf, e) => new Conjunction(mf, this.ParseExpression(e)));
    }
    
    public IModalFormula ParseOr(Or or)
    {
        return or.Expressions.Aggregate<IExpression, IModalFormula>(
            Bool.False,
            (mf, e) => new Disjunction(mf, this.ParseExpression(e)));
    }

    public IModalFormula ParseNeg(Neg neg)
    {
        return new Negation(this.ParseExpression(neg.Expression));
    }

    public IModalFormula ParseCommand(Command command)
    {
        return new Diamond(
            new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("cmd_chk", [command.MachinePartIndex, $"M{command.MachinePartIndex}'Cmd{command.Name}"]))),
            Bool.True);
    }

    public IModalFormula ParseVariable(Variable variable)
    {
        return this.SingleVar(variable);
    }

    private IModalFormula GetPhiPattern(IModalFormula mf, IModalFormula sigma)
    {
        return this.Type switch
        {
            PhiType.Pos => new Implication(
                mf,
                new Box(Bool.True, sigma)),
            PhiType.Neg => new NuFixPoint(
                "P_1",
                new Conjunction(
                    new Implication(
                        new Negation(mf), // If
                        new Box(Bool.True, new FixPoint("P_1"))),
                    sigma)),
            PhiType.Fix => new Implication(
                    new Negation(mf), // If
                    new Box(Bool.True, sigma)),
            _ => throw new ArgumentException()
        };
    }

    private IModalFormula SingleVar(Variable var)
    {
        var lhsName = $"state_M{var.LHS.MachineIndex}'{var.LHS.Name}";
        var domain = var.Domain == Domain.BOOL.Name ? Domain.BOOL : Domain.INT;

        if (var.RHS is ModelInfo rhsInfo)
        {
            var rhsName = $"state_M{rhsInfo.MachineIndex}'{rhsInfo.Name}";

            return new Exists<IModalFormula>(
                "s_1,s_2",
                domain,
                new Conjunction(
                    new Diamond(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(lhsName, ["s_1"]))), Bool.True),
                    new Conjunction(
                        new Diamond(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(rhsName, ["s_2"]))), Bool.True),
                        new BooleanExp($"s_1 {var.Operand} s_2"))));
        }

        return new Exists<IModalFormula>(
            "s_1",
            domain,
            new Conjunction(
                new Diamond(
                    new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(lhsName, ["s_1"]))),
                    Bool.True),
                new BooleanExp($"s_1 {var.Operand} {var.RHS}")));
    }

    public IModalFormula Flatten() => this;

    public string ToLatex() => $@"\phi_{this.Type}_{this.Sigma.ToLatex()}";

    public string ToMCRL2() => $"phi_{this.Type}_{this.Sigma.ToMCRL2()}";
}
