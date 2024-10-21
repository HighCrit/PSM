using PSM.Common.PROPEL;
using System.Runtime.CompilerServices;

namespace PSM.Constructors.PROPEL2DNL
{
    public static class DNLCatalogue
    {
        public static string GetScope(Scope scope, Option option)
        {
            return scope switch
            {
                Scope.Global => "From the start of any event sequence through to the end of that event sequence, the behaviour must hold.",
                Scope.Between => GetBetween(option),
                _ => $"Missing DNL template for scope '{scope}'."
            };
        }

        private static string GetBetween(Option option)
        {
            var firstStart = option.HasFlag(Option.FirstStart);
            var optionalEnd = option.HasFlag(Option.OptionalEnd);
            var repeatable = option.HasFlag(Option.ScopeRepeatability);

            var r = "A restricted interval in the event sequence can have both a starting delimiter, START, and an " +
                "ending delimiter, END. ";

            r += $"The behaviour is required to hold from {(firstStart ? "the first" : "an")} " +
                $"occurence of START, if it ever occurs, through to the first subsequenet occurence of END, if it ever occurs. ";

            r += $"If there are multiple occurences of START without an occurence of END in between them, only the {(firstStart ? "first" : "last")}" +
                $" of those occurences of START {(optionalEnd ? string.Empty : "potentially ")}starts {(repeatable ? "a" : "the")} restricted inveral;" +
                $" {(firstStart ? $"later occurences of START {(repeatable ? "within this restricted interval " : string.Empty)}do not have an effect."
                : $"each of those occurences of START {(repeatable ? "within this restricted interval " : string.Empty)} resets the beginning of " +
                $"{(repeatable ? "this" : "the")} restricted interval.")} ";

            r += "START is not required to occur and if it never occurs, then the behaviour is not required to hold anywhere in the event sequence. " +
                $"Even if START does occur, END is not required to occur subsequently{(optionalEnd ? ". Even if END does not occur subsequently, the " +
                "behaviour is still required to hold, until the end of the event sequence. " : " and if it does not occur subsequently, then the behaviour " +
                "is not required to hold for the remainder of the event sequence.")} "; 

            if (repeatable)
            {
                r += "There might be many restricted intervals in the event sequence. If START occurs after the end of a restricted interval, then the situation would be the same as after the first occurence of START. ";
            }
            else
            {
                r += "There can be at most on restricted interval in the event sequence. ";
            }

            r += "There are no restrictions imposed on the occurences of any events outside of the restricted interval(s).";

            return r;
        }

        public static string GetBehaviour(Behaviour beh, Option option)
        {
            return beh switch
            {
                Behaviour.Absence => "A must never occur.",
                Behaviour.Existence => GetExistence(option),
                _ => $"Missing DNL template for behaviour '{beh}'."
            };
        }

        private static string GetExistence(Option option)
        {
            return $"A must occur {(option.HasFlag(Option.Bounded) ? "exactly once" : "at least once")}.";
        }
    }
}
