
namespace PSM.Parsers.Labels.Labels
{
    public class Boolean : IExpression
    {
        public static Boolean True = new Boolean(true);
        public static Boolean False = new Boolean(false);

        private bool value;

        private Boolean(bool value) { this.value = value; }

        public IEnumerable<Command> GetCommandsInSubTree() => [];

        public IEnumerable<IExpression> GetVariablesInSubTree() => [];

        public string ToMCRL2()
        {
            return this.value ? "true" : "false";
        }
    }
}
