using Cordis2Cordis.XML;
using CordisSchema;
using PSM.Common;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.PROPEL;
using PSM.Constructors.PROPEL2DNL;
using PSM.Constructors.PROPEL2MuCalc;
using PSM.Parsers.Labels;
using PSM.Parsers.Labels.Labels;
using System.Reflection.PortableExecutable;

namespace PSM
{
    public class PSMFactory
    {
        private CordisModel model;
        private List<string> commands = new();
        private List<string> variables = new();

        public PSMFactory(string modelPath)
        {
            this.model = XMLParser.Load(modelPath);

            this.PopulateOptions();
        }

        public IReadOnlyList<string> Commands => commands;
        public IReadOnlyList<string> Variables => variables;

        public string GetDNL(Behaviour b, Scope s, Option o)
        {
            return DNLCatalogue.GetScope(s, o) + Environment.NewLine + DNLCatalogue.GetBehaviour(b, o);
        }

        public IModalFormula GenerateMuCalcFormula(Behaviour b, Scope s, Option o, string A, string? B, string? START, string? END)
        {
            var pattern = PatternCatalogue.GetPattern(b, s, o);

            var substitutions = new Dictionary<Event, IExpression>()
            {
                [Event.A] = LabelParser.Parse(A) ?? throw new FormatException("Failed to parse expression for label A."),
                [Event.B] = LabelParser.Parse(B) ?? throw new FormatException("Failed to parse expression for label B."),
                [Event.Start] = LabelParser.Parse(START) ?? throw new FormatException("Failed to parse expression for label START."),
                [Event.End] = LabelParser.Parse(END) ?? throw new FormatException("Failed to parse expression for label END."),
            };

            return pattern.ApplySubstitutions(substitutions).Flatten();
        }

        private void PopulateOptions()
        {
            var mps = this.model.Packages.SelectMany(p => p.MachineParts);
            var machine = mps.Single(m => m.ClassType is "Machine");

            this.PopulateMachinePart(mps, machine, machine.Name);
        }

        private void PopulateMachinePart(IEnumerable<MachinePart> mps, MachinePart currMp, string path)
        {
            commands.AddRange(currMp.Commands?.Select(c => $"CmdChk({path}.{c.Name})") ?? []);
            variables.AddRange(currMp.Variables?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.Constants?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.Inputs?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.Outputs?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.InputSignals?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.OutputSignals?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.InOutSignals?.Select(v => $"{path}.{v.Name}") ?? []);
            variables.AddRange(currMp.Observers?.Select(v => $"{path}.{v.Name}") ?? []);

            foreach (var mpRef in currMp?.MachineParts ?? [])
            {
                var mp2 = mps.Single(m => m.ClassName == mpRef.ClassName);

                this.PopulateMachinePart(mps, mp2, $"{path}.{mpRef.Name}");
            }
        }
    }
}
