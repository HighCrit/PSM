namespace PSM.UML.SM;

public interface IState
{
    public string ID { get; }

    public StateType Type { get; }

    public IReadOnlyCollection<Transition> Transitions { get; }

    public string Label { get; }
}