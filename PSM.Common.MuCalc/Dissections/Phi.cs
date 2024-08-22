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

    public IModalFormula ToMuCalc(Dictionary<Event, IExpression>? substitutions)
    {
        var expressions = this.GetLabelExpressions(substitutions);

        if (expressions.Count == 1)
        {
            return this.ParseExpression(expressions[0]);
        }
        // Sometimes used by Neg and Fix patterns
        else if (expressions.Count == 2 && this.Type is PhiType.Neg or PhiType.Fix)
        {
            var exp = new Or(expressions[0], expressions[1]);
            return this.ParseExpression(exp);
        }

        throw new ArgumentException($"Expected 1 or 2 expressions, got {expressions.Count}");
    }

    public IModalFormula ParseExpression(IExpression expression)
    {
        return expression switch
        {
            And and => this.ParseAnd(and),
            Or or => this.ParseOr(or),
            Neg neg => this.ParseNeg(neg),
            Variable v => this.ParseVariable(v),
            Command c => this.ParseCommand(c),
            _ => throw new ArgumentException(null, nameof(expression))
        };
    }
    
    public IModalFormula ParseAnd(And and)
    {
        // Push negations inwards
        var expressions = and.Expressions.Select(this.PushNegationInwards).ToList();
        
        // Distribute AND over OR.
        if (expressions.Any(e => e is Or))
        {
            var or = expressions.OfType<Or>().First();
            var otherExps = expressions.Where(e => !e.Equals(or));

            return this.ParseExpression(new Or(
                    or.Expressions.Select(e => new And([..otherExps, e]))
                ));
        }

        // Collapse ANDs
        if (expressions.Any(e => e is And))
        {
            var innerAnds = expressions.OfType<And>();
            var otherExps = expressions.Where(e => !innerAnds.Any(i => i.Equals(e)));

            var newExps = innerAnds.SelectMany(a => a.Expressions).Concat(otherExps).ToList();
            return this.ParseExpression(new And([..newExps]));
        }

        // Command
        IActionFormula commandAf = new ActionFormula.ActionFormula(Action.True);
        if (expressions.OfType<Command>().Any())
        {
            // TODO: Support multiple commands in AND (is possible due to negation)
            commandAf = this.SingleCom(expressions.OfType<Command>().Single());
        }
        
        // Variable
        var variables = expressions.Where(e => e is Variable or Neg);
        var variableMf = variables.Aggregate((IModalFormula)Bool.True, (mf, v) =>
        {
            if (v is Neg neg) return new Conjunction(mf, new Negation(this.SingleVar((Variable)neg.Expression)));
            return new Conjunction(mf, this.SingleVar((Variable)v));
        });

        return this.GetVarForType(variableMf, commandAf);
    }

    public IModalFormula ParseOr(Or or)
    {
        var expressions = or.Expressions.Select(this.PushNegationInwards).ToList();
        
        IModalFormula? finalMf = null;
        
        var commands = expressions.OfType<Command>().ToList();
        if (commands.Any())
        {
            var commandAf = commands.Aggregate((IActionFormula)new ActionFormula.ActionFormula(Action.False), (af, c) =>
            {
                var cmd = this.SingleCom(c);
                return new Union(af, cmd);
            });

            finalMf = this.GetComBoxForType(commandAf, this.Sigma);
        }
        
        var variables = expressions.Where(e => e is Variable or Neg).ToList();
        if (variables.Any())
        {
            var variableMf = variables.Aggregate((IModalFormula)Bool.False, (mf, v) =>
            {
                if (v is Neg neg) return new Disjunction(mf, new Negation(this.SingleVar((Variable)neg.Expression)));
                return new Disjunction(mf, this.SingleVar((Variable)v));
            });
            
            finalMf = finalMf is null ? this.GetVarForType(variableMf, null) : new Disjunction(finalMf, this.GetVarForType(variableMf, null));
        }

        var otherExp = expressions.Where(e => e is And or Or).ToList();
        if (otherExp.Any())
        {
            var otherMf = otherExp.Aggregate((IModalFormula)Bool.False,
                (mf, e) => new Disjunction(mf, this.ParseExpression(e)));
            
            finalMf = finalMf is null ? otherMf : new Disjunction(finalMf, otherMf);
        }

        return finalMf!;
    }

    public IModalFormula ParseNeg(Neg neg)
    {
        return neg.Expression switch
        {
            Variable var => new Negation(this.ParseExpression(var)),
            _ => this.ParseExpression(this.PushNegationInwards(neg))
        };
    }

    public IModalFormula ParseCommand(Command command)
    {
        var af = this.SingleCom(command);
        if (command.Negated)
        {
            af = new Complement(af);
        }
        
        return this.Type switch
        {
            PhiType.Pos => new Box(new RegularFormula.ActionFormula(af), this.Sigma),
            PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(new Complement(af))), this.Sigma),
            PhiType.Fix => new Box(new RegularFormula.ActionFormula(new Complement(af)), this.Sigma),
            _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
        };
    }

    public IModalFormula ParseVariable(Variable variable) =>
        this.GetVarForType(this.SingleVar(variable), new ActionFormula.ActionFormula(Action.True));

    private IExpression PushNegationInwards(IExpression exp)
    {
        if (exp is not Neg neg) return exp;
        
        return neg.Expression switch
        {
            And and => new Or(and.Expressions.Select(e => new Neg(e))),
            Or or => new And(or.Expressions.Select(e => new Neg(e))),
            Neg innerNeg => innerNeg.Expression,
            Command cmd => cmd with { Negated = !cmd.Negated },
            Variable => neg, // Neg cannot be pushed into variables
            _ => throw new ArgumentException(null, nameof(neg.Expression))
        };
    }
    
    private IModalFormula GetComBoxForType(IActionFormula af, IModalFormula sigma, PhiType? type = null)
    {
        type ??= this.Type;
        // Only negate the af if type is neg or fix and it is not true.
        if (type is PhiType.Neg or PhiType.Fix && !af.Equals(new ActionFormula.ActionFormula(Action.True)))
        {
            af = new Complement(af);
        }

        return type switch
        {
            PhiType.Pos => new Box(new RegularFormula.ActionFormula(af), sigma),
            PhiType.Neg => new Box(new Kleene(new RegularFormula.ActionFormula(af)), sigma),
            PhiType.Fix => new Box(new RegularFormula.ActionFormula(af), sigma),
            _ => throw new ArgumentException($"Invalid Phi type '{this.Type}'")
        };
    }

    private IModalFormula GetVarForType(IModalFormula variables, IActionFormula? commands)
    {
        commands ??= new ActionFormula.ActionFormula(Action.True);

        return this.Type switch
        {
            PhiType.Pos => new Implication(variables, this.GetComBoxForType(commands, this.Sigma)),
            PhiType.Neg => new NuFixPoint("P_1",
                new Conjunction(
                    new Implication(new Negation(variables),
                        this.GetComBoxForType(commands, new FixPoint("P_1"), PhiType.Fix)), this.Sigma)),
            PhiType.Fix => new Implication(new Negation(variables), this.GetComBoxForType(commands, this.Sigma)),
            _ => throw new ArgumentException()
        };
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
