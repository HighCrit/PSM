namespace PSM.UML.SM;

public interface IStateMachine
{
    public IReadOnlyCollection<IState> States { get; }
}