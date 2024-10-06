namespace PSM.UML.SM;

public interface IState
{
    public StateType Type { get; }

    public IReadOnlyCollection<Transition> Transitions { get; }
}