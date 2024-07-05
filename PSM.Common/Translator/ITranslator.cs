using PSM.Common.UML;

namespace PSM.Common.Translator
{
    public interface ITranslator<TIn, TOut>
    {
        public TOut Translate(TIn @in);
    }
}
