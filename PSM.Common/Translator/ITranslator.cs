using PSM.Common.UML;

namespace PSM.Common.Translator
{
    public interface ITranslator<T>
    {
        public T Translate(StateMachine stateMachine);
    }
}
