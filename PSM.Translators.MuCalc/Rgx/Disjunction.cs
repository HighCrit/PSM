namespace PSM.Translators.MuCalc.Rgx;

public class Disjunction : RegexBase
{
    private RegexBase Left { get; set; }
    private RegexBase Right { get; set; }

    public Disjunction(RegexBase left, RegexBase right)
    {
        this.Left = left;
        this.Right = right;
    }

    public override string ToString()
    {
        if (this.Left == Token.Epsilon && this.Right == Token.Epsilon)
        {
            return Token.Epsilon.ToString();
        }
        if (this.Left == Token.Epsilon)
        {
            return $"{this.Right}?";
        }
        if (this.Right == Token.Epsilon)
        {
            return $"{this.Left}?";
        }
        return $"{this.Left}|{this.Right}";
    }

    public override object Clone()
    {
        return new Disjunction((RegexBase)this.Left.Clone(), (RegexBase)this.Right.Clone());
    }

    public override RegexBase Flatten()
    {
        var flattenedLeft = this.Left.Flatten();
        var flattenedRight = this.Right.Flatten();
        if (flattenedLeft == Token.Epsilon && flattenedRight == Token.Epsilon) return Token.Epsilon;
        if (flattenedLeft == Token.EmptySet && flattenedRight == Token.EmptySet) return Token.EmptySet;
        if (flattenedLeft == Token.EmptySet) return flattenedRight;
        if (flattenedRight == Token.EmptySet) return flattenedLeft;
        if (flattenedLeft == Token.Epsilon) return new Optional(flattenedRight).Flatten();
        if (flattenedRight == Token.Epsilon) return new Optional(flattenedLeft).Flatten();

        if (flattenedLeft is Disjunction or Concatenation) flattenedLeft = new Group(flattenedLeft);
        if (flattenedRight is Disjunction or Concatenation) flattenedRight = new Group(flattenedRight);
        return new Disjunction(flattenedLeft, flattenedRight);
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Disjunction disjunction)
        {
            return disjunction.Left.Equals(this.Left) && disjunction.Right.Equals(this.Right);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (this.Left, this.Right).GetHashCode();
    }
}
