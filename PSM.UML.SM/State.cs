namespace PSM.UML.SM;

public class State : IState
{
    private readonly HashSet<Transition> transitions = new HashSet<Transition>();

    public State(StateType type, string? stereotype = null, string text = "")
    {
        this.Type = type;
        this.Stereotype = stereotype;
        this.Text = text;
    }

    public string ID { get; } = Guid.NewGuid().ToString().Replace("-", string.Empty);

    public string? Stereotype { get; }

    public StateType Type { get; }

    public IReadOnlyCollection<Transition> Transitions => this.transitions;

    public string Text { get; }

    public string Label => 
        string.IsNullOrWhiteSpace(this.Stereotype) ? this.Text : $"<<{this.Stereotype}>>{Environment.NewLine}{this.Text}";

    public bool AddTransition(State target, string label)
    {
        return this.AddTransition(new Transition(target, label));
    }

    public bool AddTransition(Transition transition)
    {
        return this.transitions.Add(transition);
    }
}