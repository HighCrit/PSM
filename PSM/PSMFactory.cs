using System.Collections.Immutable;
using Cordis2Cordis.XML;
using CordisSchema;
using PSM.Common;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.PROPEL;
using PSM.Constructors.PROPEL2DNL;
using PSM.Constructors.PROPEL2MuCalc;
using PSM.Parsers.Labels;
using PSM.Parsers.Labels.Labels;
using Cordis2Cordis.Preprocess;
using System.Xml.Linq;

namespace PSM
{
    public class PSMFactory
    {
        private CordisModel model;

        private Dictionary<string, ModelInfo> atlas = new();

        public PSMFactory(string modelPath)
        {
            this.model = XMLParser.Load(modelPath);

            this.PopulateOptions();
        }

        public IReadOnlyDictionary<string, ModelInfo> Atlas => this.atlas;

        public string GetDNL(Behaviour b, Scope s, Option o)
        {
            return DNLCatalogue.GetScope(s, o) + Environment.NewLine + DNLCatalogue.GetBehaviour(b, o);
        }

        public IModalFormula GenerateMuCalcFormula(Behaviour b, Scope s, Option o, string A, string? B, string? START, string? END)
        {
            var pattern = PatternCatalogue.GetPattern(b, s, o);

            var substitutions = new Dictionary<Event, IExpression>()
            {
                [Event.A] = LabelParser.Parse(this.atlas, A) ?? throw new FormatException("Failed to parse expression for label A."),
                [Event.B] = LabelParser.Parse(this.atlas, B) ?? throw new FormatException("Failed to parse expression for label B."),
                [Event.Start] = LabelParser.Parse(this.atlas, START) ?? throw new FormatException("Failed to parse expression for label START."),
                [Event.End] = LabelParser.Parse(this.atlas, END) ?? throw new FormatException("Failed to parse expression for label END."),
            };

            return pattern.ApplySubstitutions(substitutions).Flatten();
        }

        private void PopulateOptions()
        {
            var mps = Utility.AllMachineParts(this.model).ToList();
            var machine = mps.Single(m => m.ClassType is "Machine");

            this.TraverseMp(mps, machine, 0, machine.Name);
        }

        private int TraverseMp(List<MachinePart> allMps, MachinePart currMp, int index, string path)
        {
            index++;

            void AddVar(string name, string type)
            {
                this.atlas.Add($"{path}.{name}", new ModelInfo(ModelPropertyType.Variable, index, name, type));
            }

            foreach (var v in currMp.Variables ?? [])
            {
                AddVar(v.Name, v.Type);
            }
            foreach (var v in currMp.InputSignals ?? [])
            {
                AddVar(v.Name, v.Type);
            }
            foreach (var v in currMp.OutputSignals ?? [])
            {
                AddVar(v.Name, v.Type);
            }
            foreach (var v in currMp.InOutSignals ?? [])
            {
                AddVar(v.Name, v.Type);
            }
            foreach (var v in currMp.Inputs ?? [])
            {
                AddVar(v.Name, v.Type);
            }
            foreach (var v in currMp.Constants ?? [])
            {
                AddVar(v.Name, v.Type);
            }

            foreach (var v in currMp.Commands ?? [])
            {
                var name = v.Name;
                this.atlas.Add($"{path}.{name}", new ModelInfo(ModelPropertyType.Command, index, name));
            }

            var mpRefs = (currMp.MachineParts ?? []).ToList();
            mpRefs.Sort((l, r) => int.Parse(l.ExecutionOrder).CompareTo(int.Parse(r.ExecutionOrder)));

            foreach (var mpRef in mpRefs)
            {
                var mp = allMps.Single(m => m.ClassName == mpRef.ClassName);
                index = this.TraverseMp(allMps, mp, index, $"{path}.{mpRef.Name}");
            }

            return index;
        }
    }
}
