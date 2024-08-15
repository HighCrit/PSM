namespace PSM.Common;

public static class EnumExtensions
{
    public static IEnumerable<T> GetFlags<T>(this T input) where T : Enum
    {
        var zero = (T)Convert.ChangeType(0, Enum.GetUnderlyingType(typeof(T)));
        return Enum.GetValues(input.GetType()).Cast<T>().Where(e => !object.Equals(e, zero) && input.HasFlag(e));
    }

    public static IEnumerable<T> GetAllCombinations<T>(this IEnumerable<T> input) where T : Enum
    {
        var zero = (T)Convert.ChangeType(0, Enum.GetUnderlyingType(typeof(T)));
        var values = input.ToArray();

        return Enumerable
            .Range(0, 1 << (values.Length))
            .Select(index => values
               .Where((v, i) => (index & (1 << i)) != 0))
                .Select(a => a.Aggregate(zero, (acc, @new) => (T)(object)((int)(object)acc | (int)(object)@new)));
    }

    public static bool IsSubsetOf(this int sub, int super)
    {
        return (sub & ~super) == 0;
    }
}
