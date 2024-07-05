using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.Translator;
using PSM.Common.UML;
using PSM.Translators.MuCalc.PROPEL;

namespace PSM.Translators.MuCalc
{
    public class TranslateToMuCalc : ITranslator<StateMachine, (TemplateInfo Info, ModalFormulaBase Formula)>
    {
        private TemplatePatternGenerator templatePatternGenerator;
        private List<(string Signature, TemplateInfo Info)> TemplateSignatures;

        public TranslateToMuCalc()
        {
            this.templatePatternGenerator = new TemplatePatternGenerator();
            var test = this.templatePatternGenerator.TemplateSignatures.ToList();
            this.TemplateSignatures = this.templatePatternGenerator.TemplateSignatures.ToList();
        }

        public (TemplateInfo Info, ModalFormulaBase Formula) Translate(StateMachine stateMachine)
        {
            var rgx = SMToREConverter.ToRegEx(stateMachine).Flatten();
            var signature = rgx.ToString(true);

            var matchedTemplates = this.TemplateSignatures.Where(t => t.Signature == signature).ToList();
            if (matchedTemplates.Count == 0)
            {
                throw new ArgumentException($"Provided state-machine does not yield known signature, got: '{signature}'.");
            }

            return (matchedTemplates[0].Info, null);
        }
    }
}
