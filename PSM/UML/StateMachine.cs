namespace PSM.UML;

public class StateMachine {
    private Dictionary<string, State> States { get; } = new();
    
    public State FindOrCreate(string name) {}
}
