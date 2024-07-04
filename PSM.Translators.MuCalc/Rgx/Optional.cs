namespace PSM.Translators.MuCalc.Rgx;

public class Optional : RegexBase
{
    public RegexBase Content { get; }

    public Optional(RegexBase content)
    {
        this.Content = content;
    }

    public override object Clone()
    {
        return new Optional((RegexBase)this.Content.Clone());
    }

    public override RegexBase Flatten()
    {
        var flattened = this.Content.Flatten();
        if (flattened == Token.Epsilon) return Token.Epsilon;
        if (flattened == Token.EmptySet) return Token.EmptySet;
        if (flattened is Optional) return flattened;

        if (flattened is Disjunction or Concatenation) flattened = new Group(flattened);
        return new Optional(flattened);
    }

    public override string ToString()
    {
        if (this.Content == Token.Epsilon) return Token.Epsilon.ToString();
        if (this.Content == Token.EmptySet) return Token.EmptySet.ToString();
        return $"{this.Content}?";
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Optional optional)
        {
            return this.Content.Equals(optional.Content);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.Content.GetHashCode();
    }
}
