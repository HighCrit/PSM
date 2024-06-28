namespace PSM.Translators.MuCalc.Rgx
{
    public class Kleene : RegexBase
    {
        private RegexBase Content { get; set; }

        public Kleene(RegexBase regexBase)
        {
            this.Content = regexBase;  
        }

        public override string ToString()
        {
            return $"{this.Content}*";
        }
    }
}
