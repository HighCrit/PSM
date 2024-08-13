namespace PSM.Translators.MuCalc.Rgx;

public class Concatenation : RegexBase
{
    private RegexBase Left { get; set; }
    private RegexBase Right { get; set; }

    public Concatenation(RegexBase left, RegexBase right)
    {
        this.Left = left;
        this.Right = right;
    }

    public override string ToString(bool signature)
    {
        if (this.Left == Token.Epsilon)
        {
            return this.Right.ToString(signature);
        }   
        if (this.Right == Token.Epsilon)
        {
            return this.Left.ToString(signature);
        }
        return $"{this.Left.ToString(signature)}{this.Right.ToString(signature)}";
    }

    public override object Clone()
    {
        return new Concatenation((RegexBase)this.Left.Clone(), (RegexBase)this.Right.Clone());
    }

    public override RegexBase Flatten()
    {
        var flattenedLeft = this.Left.Flatten();
        var flattenedRight = this.Right.Flatten();
        if (flattenedLeft == Token.EmptySet || flattenedRight == Token.EmptySet) return Token.EmptySet;
        if (flattenedLeft == Token.Epsilon && flattenedRight == Token.Epsilon) return Token.Epsilon;
        if (flattenedLeft == Token.Epsilon) return flattenedRight;
        if (flattenedRight == Token.Epsilon) return flattenedLeft;
        return new Concatenation(flattenedLeft, flattenedRight);
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Concatenation concatenation)
        {
            return concatenation.Left.Equals(this.Left) && concatenation.Right.Equals(this.Right);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (this.Left, this.Right).GetHashCode();
    }
}
