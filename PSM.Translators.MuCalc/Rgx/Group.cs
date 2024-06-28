namespace PSM.Translators.MuCalc.Rgx
{
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
    }
}
