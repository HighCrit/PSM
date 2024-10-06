namespace PSM.UML.SM;

public class StateMachine : IStateMachine
{
    private readonly List<State> states = new();

    public IReadOnlyCollection<IState> States => this.states;

    public void AddState(State state)
    {
        this.states.Add(state);
    }

    public void AddStates(params State[] ss) => this.AddStates(ss);

    public void AddStates(IEnumerable<State> ss)
    {
        this.states.AddRange(ss);
    }
}