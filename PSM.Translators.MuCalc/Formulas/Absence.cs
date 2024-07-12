using System.Collections.ObjectModel;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.Common;
using PSM.Common.PROPEL;
using PSM.Translators.PROPEL;

namespace PSM.Translators.MuCalc.Formulas;

public class Absence : IBehaviourFormula
{
    #region Base Formulas
    private IReadOnlyDictionary<Option, IModalFormula> GlobalBase { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> AfterBase { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> BeforeBase { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> BetweenBase { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };
    #endregion

    #region Constraints
    private IReadOnlyDictionary<Option, IModalFormula> GlobalConstraints { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> AfterConstraints { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> BeforeConstraints { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };

    private IReadOnlyDictionary<Option, IModalFormula> BetweenConstraints { get; } = new Dictionary<Option, IModalFormula>()
    {
        [Option.None] = BooleanExp.True
    };
    #endregion

    public IModalFormula GetFormulaFor(Scope scope, Option option)
    {
        var (baseOptions, baseFormula) = scope switch
        {
            Scope.Global => this.GetBaseFrom(this.GlobalBase, option),
            Scope.After_Q => this.GetBaseFrom(this.AfterBase, option),
            Scope.Before_P => this.GetBaseFrom(this.BeforeBase, option),
            Scope.Between_Q_and_P => this.GetBaseFrom(this.BetweenBase, option),
            _ => throw new ArgumentException(nameof(scope))
        };

        var excessOptions = (baseOptions ^ option).GetFlags();
        foreach (var excessOption in excessOptions)
        {
            var constraintFormula
        }
    }

    private (Option, IModalFormula) GetBaseFrom(IReadOnlyDictionary<Option, IModalFormula> bases, Option option)
    {
        var res = bases.First(kvp => option.IsSubsetOf(kvp.Key));
        return (res.Key, res.Value);
    }
}