namespace PSM.Translators.MuCalc.Rgx
{
    public class Concatenation : RegexBase
    {
        private RegexBase Left { get; set; }
        private RegexBase Right { get; set; }

        public Concatenation(RegexBase left, RegexBase right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"{Left}{Right}";
        }
    }
}
