using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.Translator;
using PSM.Common.UML;
using PSM.Translators.MuCalc.PROPEL;

namespace PSM.Translators.MuCalc
{
    public class TranslateToMuCalc : ITranslator<StateMachine, (TemplateInfo Info, ModalFormulaBase Formula)>
    {
        private TemplatePatternGenerator templatePatternGenerator;
        private Dictionary<string, TemplateInfo> TemplateSignatures;

        public TranslateToMuCalc()
        {
            this.templatePatternGenerator = new TemplatePatternGenerator();
            var test = this.templatePatternGenerator.TemplateSignatures.ToList();
            this.TemplateSignatures = this.templatePatternGenerator.TemplateSignatures.ToDictionary();
        }

        public (TemplateInfo Info, ModalFormulaBase Formula) Translate(StateMachine stateMachine)
        {
            var rgx = SMToREConverter.ToRegEx(stateMachine).Flatten();
            var signature = rgx.ToString(true);

            if (!this.TemplateSignatures.TryGetValue(signature, out var propertyInfo))
            {
                throw new ArgumentException($"Provided state-machine does not yield known signature, got: '{signature}'.");
            }

            return (propertyInfo, null);
        }
    }
}
