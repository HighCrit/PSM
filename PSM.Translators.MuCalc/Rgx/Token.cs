namespace PSM.Translators.MuCalc.Rgx
{
    public class Token : RegexBase
    {
        private string Content { get; set; }

        public Token(string token)
        {
            this.Content = token;
        }

        public override string ToString()
        {
            return this.Content;
        }
    }
}
