namespace PSM.Common.PROPEL;

/// <summary>
/// An enum containing the available scope-behaviour options.
/// </summary>
[Flags]
public enum Option
{
    /// <summary>
    /// Denotes that no behaviour options are selected.
    /// </summary>
    None = 0,

    /// <summary>
    /// Whether an <see cref="Event.A"/> may occur only once or more.
    /// </summary>
    Bounded = 1,

    /// <summary>
    /// Whether an <see cref="Event.A"/> is required to occur.
    /// </summary>
    Nullity = 2,

    /// <summary>
    /// Whether an <see cref="Event.B"/> is allowed to occur before the first <see cref="Event.A"/>.
    /// </summary>
    Precedency = 4,

    /// <summary>
    /// Whether after an <see cref="Event.A"/> occured, if <see cref="Event.A"/> may occur again before the first
    /// <see cref="Event.B"/> occurs.
    /// </summary>
    PreArity = 8,

    /// <summary>
    /// Whether <see cref="Event.B"/> must immediately follow <see cref="Event.A"/>.
    /// </summary>
    Immediacy = 16,

    /// <summary>
    /// Whether after <see cref="Event.A"/> and <see cref="Event.B"/> occured, if <see cref="Event.B"/> may
    /// occur again.
    /// </summary>
    PostArity = 32,

    /// <summary>
    /// Whether after <see cref="Event.A"/> and <see cref="Event.B"/> occured, if <see cref="Event.A"/> may
    /// not occur again.
    /// </summary>
    Finalisation = 64,

    /// <summary>
    /// Whether after <see cref="Event.A"/> and <see cref="Event.B"/> occured, if the property should hold again for
    /// the next occurence of <see cref="Event.A"/>.
    /// </summary>
    Repeatability = 128,

    /// <summary>
    /// Whether the next occurence of the scope should also hold.
    /// </summary>
    ScopeRepeatability = 256,

    /// <summary>
    /// Whether the restricted interval starts at the first or last <see cref="Event.Start"/>.
    /// </summary>
    FirstStart = 512,

    /// <summary>
    /// Whether it is allowed for <see cref="Event.End"/> to not occur.
    /// </summary>
    OptionalEnd = 1024,
}
