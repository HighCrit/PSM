﻿using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.Translator;
using PSM.Common.UML;

namespace PSM.Translators.MuCalc
{
    public class TranslateToMuCalc : ITranslator<ModalFormulaBase>
    {
        public ModalFormulaBase Translate(StateMachine stateMachine)
        {
            var rgx = SMToREConverter.ToRegEx(stateMachine).Flatten();

            return null;
        }
    }
}
