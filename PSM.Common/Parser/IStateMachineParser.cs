using PSM.Common.UML;

namespace PSM.Common.Parser
{
    public interface IStateMachineParser
    {
        IDictionary<string, StateMachine> Parse(string filePath);
    }
}
