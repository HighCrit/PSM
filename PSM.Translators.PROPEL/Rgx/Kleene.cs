namespace PSM.Translators.MuCalc.Rgx;

public class Kleene : RegexBase
{
    private RegexBase Content { get; set; }

    public Kleene(RegexBase regexBase)
    {
        this.Content = regexBase;  
    }

    public override string ToString(bool signature)
    {
        if (this.Content == Token.Epsilon)
        {
            return Token.Epsilon.ToString(signature);
        }
        if (this.Content == Token.EmptySet)
        {
            return Token.EmptySet.ToString(signature);
        }
        return $"{this.Content.ToString(signature)}*";
    }

    public override object Clone()
    {
        return new Kleene((RegexBase)this.Content.Clone());
    }

    public override RegexBase Flatten()
    {
        var flattened = this.Content.Flatten();
        if (flattened == Token.Epsilon) return Token.Epsilon;
        if (flattened == Token.EmptySet) return Token.EmptySet;
        if (flattened is Optional opt) return new Kleene(opt.Content);

        if (flattened is Disjunction or Concatenation) flattened = new Group(flattened);
        return new Kleene(flattened);
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Kleene kleene)
        {
            return this.Content.Equals(kleene.Content);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.Content.GetHashCode();
    }
}
