using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSM.Translators.MuCalc.Rgx
{
    public class Disjunction : RegexBase
    {
        private RegexBase Left { get; set; }
        private RegexBase Right { get; set; }

        public Disjunction(RegexBase left, RegexBase right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"{Left}|{Right}";
        }
    }
}
