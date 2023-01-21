using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Extensions;

public static class EnumerableExtensions
{
    // Example Using: var intersection = (new[] { list1, list2, list3 }).IntersectMany(l => l).ToList();
    public static IEnumerable<TResult> IntersectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                return new TResult[0];

            var ret = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                ret = ret.Intersect(selector(enumerator.Current));
            }

            return ret;
        }
    }
}