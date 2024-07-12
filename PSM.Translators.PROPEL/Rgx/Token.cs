namespace PSM.Translators.MuCalc.Rgx;

public class Token(string token) : RegexBase
{
    public static readonly Token EmptySet = new("Ø");
    public static readonly Token All = new(".");
    public static readonly Token Epsilon = new("ε");

    private string Content { get; set; } = token;

    public override string ToString(bool signature)
    {
        return signature && !(
            this.Equals(Token.Epsilon) ||
            this.Equals(Token.EmptySet) ||
            this.Equals(Token.All)) 
            ? "T" : this.Content;
    }

    public override object Clone()
    {
        return new Token(this.Content);
    }

    public override RegexBase Flatten()
    {
        return this;
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Token token)
        {
            return token.Content == this.Content;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.Content.GetHashCode();
    }
}
