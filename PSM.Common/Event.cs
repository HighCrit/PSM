namespace PSM.Common;

/// <summary>
/// An enum containing all the event tokens
/// </summary>
[Flags]
public enum Event
{
    /// <summary>
    /// Denotes the starting delimiter.
    /// </summary>
    Start = 1 << 1,

    /// <summary>
    /// Denotes the first event of the behaviour.
    /// </summary>
    A = 1 << 2,

    /// <summary>
    /// Denotes the secondary event of the behaviour/
    /// </summary>
    B = 1 << 3,

    /// <summary>
    /// Denotes the end delimiter.
    /// </summary>
    End = 1 << 4,
}
