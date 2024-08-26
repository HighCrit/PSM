﻿// <copyright file="LabelExtensions.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels.Operations;

namespace PSM.Common.MuCalc.Dissections.Labels;

public static class LabelExtensions
{
    public static IExpression ToDNF(this IExpression expression)
    {
        var pushedInNegation = PushNegationInwards(expression);
        var distributedAnd = DistributeAndOverOr(pushedInNegation);

        return distributedAnd;
    }

    private static IExpression PushNegationInwards(IExpression expression)
    {
        if (expression is Neg neg)
        {
            return neg.Expression switch
            {
                And a => new Or(a.Expressions.Select(e => PushNegationInwards(new Neg(e)))),
                Or or => new And(or.Expressions.Select(e => PushNegationInwards(new Neg(e)))),
                Neg innerNeg => PushNegationInwards(innerNeg.Expression),
                Command cmd => throw new ArgumentException("Command may not occur negatively"),
                Variable => neg, // Neg cannot be pushed into variables
                _ => throw new ArgumentException(null, nameof(neg.Expression))
            };
        }

        return expression;
    }

    private static IExpression DistributeAndOverOr(IExpression expression)
    {
        if (expression is And and && and.Expressions.OfType<Or>().Any())
        {
            var or = and.Expressions.OfType<Or>().First();
            var otherExps = and.Expressions.Where(e => !e.Equals(or));

            return new Or(
                or.Expressions.Select(e => DistributeAndOverOr(new And([.. otherExps, e]))));
        }

        return expression;
    }
}