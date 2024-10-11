using System.Text.RegularExpressions;

namespace PSM.Parsers.mCRL2Parser
{
    public static class MCRL2Parser
    {
        public static readonly Regex COMMAND_REGEX = new Regex("M3'Cmd(?<name>\\w+)", RegexOptions.Compiled);

        public static IDictionary<string, string> ParseCommandIds(string modelPath)
        {
            var model = File.ReadAllText(modelPath);
            var atlas = new Dictionary<string, string>();

            foreach (Match match in COMMAND_REGEX.Matches(model))
            {
                atlas[match.Groups["name"].Value] = match.Value;
            }

            return atlas;
        } 
    }
}
