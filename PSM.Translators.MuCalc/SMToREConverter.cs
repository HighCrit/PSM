using PSM.Common.UML;
using PSM.Translators.MuCalc.Rgx;
using System.Linq;

namespace PSM.Translators.MuCalc;

public static class SMToREConverter
{
    /// <summary>
    /// Executes GNFA to RE.
    /// </summary>
    /// <param name="sm">A FSA based state-machine.</param>
    /// <returns>The corresponding RE.</returns>
    public static RegexBase ToRegEx(this StateMachine sm)
    {
        StateMachine copySm = (StateMachine)sm.Clone();

        copySm.UnifyAcceptingStates();
        copySm.MakeFullConnected();

        var queue = new Queue<string>();
        var initial = copySm.States.Values.Single(s => (s.Type & StateType.Initial) is StateType.Initial);
        var visited = new HashSet<string>();
        var initialSucc = initial.Transitions.Select(t => t.Target).Distinct().ToList();
        initialSucc.ForEach(s =>
        {
            queue.Enqueue(s);
            visited.Add(s);
        });

        while (queue.Count > 0 && copySm.States.Count > 2)
        {
            var s = queue.Dequeue();
            var succs = copySm.FindOrCreate(s).Transitions
                .Select(t => copySm.FindOrCreate(t.Target))
                .Where(state => 
                    (state.Type & StateType.Final) is not StateType.Final && 
                    (state.Type & StateType.Initial) is not StateType.Initial &&
                    !visited.Contains(state.Name))
                .Select(state => state.Name)
                .Distinct()
                .ToList();
            succs.ForEach(queue.Enqueue);

            copySm.PruneState(s);
        }

        return initial.Transitions.Single().Label?.ToRE() ?? Token.EmptySet;
    }

    private static void MakeFullConnected(this StateMachine sm)
    {
        var nonFinalStates = sm.States.Values.Where(s => (s.Type & StateType.Final) is not StateType.Final).ToList();

        foreach (var s1 in nonFinalStates) foreach (var s2 in nonFinalStates)
        {
            // A transition already exists
            if (s1.Transitions.Any(t => t.Target.Equals(s2)) || (s2.Type & StateType.Initial) is StateType.Initial) continue;
            // Add self-loop
            if (s1.Equals(s2)) s1.AddTransition(s1.Name, Token.Epsilon);
            // Add non-accepting transition otherwise.
            else s1.AddTransition(s2.Name, Token.EmptySet);
        }
    }

    private static void UnifyAcceptingStates(this StateMachine sm)
    {
        // In PSM accepting states are non-initial, non-invalid states.
        var accepting = sm.States.Values.Where(s => s.Type is StateType.Normal or StateType.Final);

        var unifiedAcceptingState = sm.FindOrCreate(Guid.NewGuid().ToString());

        foreach (var s in accepting)
        {
            if (s.Transitions.Any(t => t.Target.Equals(unifiedAcceptingState))) continue;
            s.AddTransition(unifiedAcceptingState.Name, Token.Epsilon);
            s.Type = StateType.Invalid;
        }
    }

    private static void PruneState(this StateMachine sm, string rip)
    {
        var ripState = sm.FindOrCreate(rip);

        var successorsT = ripState.Transitions
            .Where(t => !t.Target.Equals(rip)) // Exclude selfloop
            .ToList();
        var predecessorsT = sm.States.Values.SelectMany(s => s.Transitions)
            .Where(t => !t.Source.Equals(rip) && t.Target.Equals(rip))
            .ToList();
        var selfLoops = ripState.Transitions.Where(t => t.Target.Equals(rip));

        var selfLoopRE = selfLoops.Aggregate<Transition, RegexBase>(Token.Epsilon, (re, t) =>
        {
            return new Disjunction(re, t.Label?.ToRE() ?? Token.Epsilon);
        });
        selfLoopRE = new Kleene(new Group(selfLoopRE));

        foreach (var tPred in predecessorsT) 
            foreach (var tSucc in successorsT)
            {
                var predRe = tPred.Label?.ToRE() ?? Token.Epsilon;
                var succRe = tSucc.Label?.ToRE() ?? Token.Epsilon;

                var transRe = new Concatenation(predRe, new Concatenation(selfLoopRE, succRe));

                sm.FindOrCreate(tPred.Source).RemoveTransition(tPred);
                sm.FindOrCreate(tPred.Source).AddTransition(tSucc.Target, transRe);
            }

        foreach (var tPred in predecessorsT)
        {

            sm.FindOrCreate(tPred.Source).MergeTransitions();
        }

        sm.Remove(rip);
    }

    private static void MergeTransitions(this State state)
    {
        var duplicates = state.Transitions.Where(t1 => state.Transitions.Any(t2 => !t1.Equals(t2) && t1.Target.Equals(t2.Target))).ToList();

        for (var i = 0; i < duplicates.Count - 1; i++) for (var j = i + 1; j < duplicates.Count; j++)
            {
                var t1 = duplicates[i];
                var t2 = duplicates[j];

                if (t1.Target == t2.Target)
                {
                    t1.Label = new Disjunction(t1.Label?.ToRE() ?? Token.Epsilon, t2.Label?.ToRE() ?? Token.Epsilon);
                    state.RemoveTransition(t2);
                }
            }
    }

    private static RegexBase ToRE(this Label label)
    {
        if (label is RegexBase reb)
        {
            return reb;
        }

        return new Token(label.ToString()!);
    }
}
