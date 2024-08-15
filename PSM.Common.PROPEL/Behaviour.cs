namespace PSM.Common.PROPEL;

/// <summary>
/// The available PROPEL behaviours.
/// </summary>
public enum Behaviour
{
    /// <summary>
    /// During the restricted interval no <see cref="Event.A"/> may occur.
    /// </summary>
    Absence,

    /// <summary>
    /// During the restricted interval at least/exactly one occurence of <see cref="Event.A"/> must occur.
    /// </summary>
    Existence,

    /// <summary>
    /// During the restricted interval <see cref="Event.A"/> must occur before <see cref="Event.B"/> may occur subsequently.
    /// </summary>
    Precedence,

    /// <summary>
    /// During the restricted interval <see cref="Event.B"/> is required to occur after an occurence of <see cref="Event.A"/>.
    /// </summary>
    Response,
}
