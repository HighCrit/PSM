namespace PSM.UML;

public class State(string name, IEnumerable<Transition> transitions) {
    public string Name { get; private set; } = name;
    public List<Transition> Transitions { get; private set; } = transitions.ToList();

    public State AddTransition(State target, string label)
    {
        this.Transitions.Add(new Transition(this, target, label));
        return this;
    }
}
