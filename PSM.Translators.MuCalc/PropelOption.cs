namespace PSM.Translators.MuCalc;

[Flags]
public enum PropelOption
{
    Bounded = 1,
    Nullity = 2,
    Precedency = 4,
    Pre_arity = 8,
    Immediacy = 16,
    Post_arity = 32,
    Finalisation = 64,
    Repeatability = 128,
}
