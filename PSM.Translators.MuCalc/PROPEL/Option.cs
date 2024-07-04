namespace PSM.Translators.MuCalc.PROPEL;

[Flags]
public enum Option
{
    None = 0,

    /// <summary>
    /// Whether an action A may occur only once or more.
    /// </summary>
    Bounded = 1,

    /// <summary>
    /// Whether an action A is required to occur.
    /// </summary>
    Nullity = 2,

    /// <summary>
    /// Whether an action B is allowed to occur before A.
    /// </summary>
    Precedency = 4,

    /// <summary>
    /// Whether after an A occured, if A may occur again before the first action B occurs.
    /// </summary>
    Pre_arity = 8,

    /// <summary>
    /// Whether action B must immediately follow action A or if other actions are allowed to occur in between.
    /// </summary>
    Immediacy = 16,

    /// <summary>
    /// Whether after actions A and B occured, if B may occur again.
    /// </summary>
    Post_arity = 32,

    /// <summary>
    /// Whether after action A and B occured, if A may occur again.
    /// </summary>
    Finalisation = 64,

    /// <summary>
    /// Whether after actions A and B occured, if the property should hold again for the next occurence of A.
    /// </summary>
    Repeatability = 128,
}
