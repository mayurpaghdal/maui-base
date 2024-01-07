namespace MauiAppDemo.Extensions;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class EnumerableExtensions
{
    internal static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T item)
    {
        foreach (T x in enumerable)
            yield return x;

        yield return item;
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static IDictionary<TKey, List<TSource>> GroupToDictionary<TSource, TKey>(this IEnumerable<TSource> enumeration, Func<TSource, TKey> func)
    {
        var result = new Dictionary<TKey, List<TSource>>();
        foreach (TSource item in enumeration)
        {
            var group = func(item);
            if (!result.ContainsKey(group))
                result.Add(group, new List<TSource> { item });
            else
                result[group].Add(item);
        }
        return result;
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, T item)
    {
        if (enumerable == null)
            throw new ArgumentNullException("enumerable");

        var i = 0;
        foreach (T element in enumerable)
        {
            if (Equals(element, item))
                return i;

            i++;
        }

        return -1;
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        var i = 0;
        foreach (T element in enumerable)
        {
            if (predicate(element))
                return i;

            i++;
        }

        return -1;
    }

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> enumerable, T item)
    {
        yield return item;

        foreach (T x in enumerable)
            yield return x;
    }
}
