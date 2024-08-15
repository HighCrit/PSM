namespace PSM.Common.PROPEL;

/// <summary>
/// An enum containing the available scopes.
/// </summary>
public enum Scope
{
    /// <summary>
    /// Denotes that the behaviour must always hold.
    /// </summary>
    Global,

    /// <summary>
    /// Denotes that the behaviour must hold starting at an occurence of <see cref="Event.Start"/> till the end of the sequence.
    /// </summary>
    After,

    /// <summary>
    /// Denotes that the behaviour must hold before or until an occurence of <see cref="Event.End"/>.
    /// </summary>
    Before,

    /// <summary>
    /// Denotes that the behaviour must hold starting at an occurence of <see cref="Event.Start"/> till the end of the event sequence
    /// or an occurence of <see cref="Event.End"/>.
    /// </summary>
    Between,
}
