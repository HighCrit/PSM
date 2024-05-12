namespace PSM.UML;

public class Transition(State source, State target, string label)
{
    public State Source { get; private set; } = source;
    public State Target { get; private set; } = target;
    public string Label { get; private set; } = label;
}
