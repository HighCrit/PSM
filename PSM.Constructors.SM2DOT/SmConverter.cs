using System.Diagnostics;
using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using PSM.UML.SM;

namespace PSM.Constructors.SM2DOT
{
    public class SmConverter
    {
        /// <summary>
        /// Converts a state-machine to graphviz's DOT language, generates the PNG and returns the path to the image.
        /// </summary>
        /// <param name="sm">The state-machine to convert.</param>
        /// <returns>Path to image.</returns>
        public static string ToPNG(IStateMachine sm)
        {
            var graph = new DotGraph().WithIdentifier("PSM").Directed();

            foreach (var s in sm.States)
            {
                var stateNode = new DotNode()
                    .WithIdentifier(s.ID)
                    .WithLabel(s.Label);

                if (s.Type is StateType.Initial)
                {
                    stateNode
                        .WithStyle(DotNodeStyle.Filled)
                        .WithFillColor(DotColor.Black)
                        .WithShape(DotNodeShape.Circle);
                }
                else
                {
                    stateNode.WithShape(DotNodeShape.Rectangle);
                }

                graph.Add(stateNode);

                foreach (var t in s.Transitions)
                {
                    var edge = new DotEdge()
                        .From(s.ID)
                        .To(t.Target.ID)
                        .WithArrowHead(DotEdgeArrowType.Normal)
                        .WithLabel(t.Label);

                    graph.Add(edge);
                }
            }

            using var writer = new StringWriter();
            var context = new CompilationContext(writer, new CompilationOptions());
            graph.CompileAsync(context).GetAwaiter().GetResult();

            var result = writer.GetStringBuilder().ToString();

            var cdPath = Path.Combine(Environment.CurrentDirectory, "gen");

            if (!Directory.Exists(cdPath))
            {
                Directory.CreateDirectory(cdPath);
            }

            var dotPath = Path.Combine(Environment.CurrentDirectory, "gen", "PSM.dot");
            var pngPath = Path.Combine(Environment.CurrentDirectory, "gen", "PSM.png");
            File.WriteAllText(dotPath, result);

            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = cdPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "dot.exe",
                Arguments = $"-Tpng {dotPath} -o {pngPath}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            return pngPath;
        }
    }
}
