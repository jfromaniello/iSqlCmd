using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication13
{
    public static class Extensions
    {
        public static IEnumerable<T> TakeUntil<T>(
            this IEnumerable<T> elements,
            Func<T, bool> predicate
            )
        {
            return elements.Select((x, i) => new { Item = x, Index = i })
                .TakeUntil((x, i) => predicate(x.Item))
                .Select(x => x.Item);
        }

        public static IEnumerable<T> TakeUntil<T>(
            this IEnumerable<T> elements,
            Func<T, int, bool> predicate
            )
        {
            var i = 0;
            foreach (var element in elements)
            {
                if (predicate(element, i))
                {
                    yield return element;
                    yield break;
                }
                yield return element;
                i++;
            }
        }

        public static string RemoveEmptyLines(this string content)
        {
            return string.Join(Environment.NewLine,
                               content.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}