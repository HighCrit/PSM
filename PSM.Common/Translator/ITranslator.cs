namespace PSM.Common.Translator;

/// <summary>
/// Interface for translating classes, translates from <see cref="TIn"/> to <see cref="TOut"/>.
/// </summary>
/// <typeparam name="TIn">The input type.</typeparam>
/// <typeparam name="TOut">The output type.</typeparam>
public interface ITranslator<in TIn, out TOut>
{
    /// <summary>
    /// Translates a <see cref="TIn"/> to a <see cref="TOut"/>.
    /// </summary>
    /// <param name="in">The input object.</param>
    /// <returns>The output object.</returns>
    public TOut Translate(TIn @in);
}