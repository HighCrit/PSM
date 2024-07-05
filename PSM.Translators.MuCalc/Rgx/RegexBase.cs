namespace PSM.Translators.MuCalc.Rgx;

using PSM.Common.UML;

public abstract class RegexBase : Label, ICloneable, IEquatable<RegexBase>
{
    public override string ToString() => this.ToString(signature: false);

    public abstract string ToString(bool signature);

    public abstract RegexBase Flatten();

    public abstract object Clone();

    public override bool Equals(object? obj) => this.Equals(obj as RegexBase);

    public abstract bool Equals(RegexBase? other);

    public abstract override int GetHashCode();
}
