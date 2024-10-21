using Antlr4.Runtime;

using PSM.Parsers.Labels.Labels;

namespace PSM.Parsers.Labels;

public static class LabelParser
{
    public static IExpression? Parse(string? label)
    {
        if (string.IsNullOrWhiteSpace(label)) return Labels.Boolean.True;
        
        var stream = new AntlrInputStream(label);
        var lexer = new TransitionLabelsLexer(stream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new TransitionLabelsParser(commonTokenStream);

        var labelContext = parser.label();
        var visitor = new TransitionLabelVisitor();

        return visitor.Visit(labelContext);
    }
}