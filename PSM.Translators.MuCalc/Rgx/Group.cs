namespace PSM.Translators.MuCalc.Rgx;

internal class Group : RegexBase
{
    private RegexBase Content { get; set; }

    public Group(RegexBase content)
    {
        this.Content = content;
    }

    public override string ToString()
    {
        return $"({this.Content})";
    }

    public override object Clone()
    {
        return new Group((RegexBase)this.Content.Clone());
    }

    public override RegexBase Flatten()
    {
        var flattened = this.Content.Flatten();
        if (flattened is Token or Optional) return flattened;
        return new Group(flattened);
    }

    public override bool Equals(RegexBase? other)
    {
        if (other is not null and Group group)
        {
            return this.Content.Equals(group.Content);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.Content.GetHashCode();
    }
}
