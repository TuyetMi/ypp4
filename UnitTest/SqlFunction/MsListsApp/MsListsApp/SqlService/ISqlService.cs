using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.SqlService
{
    public interface ISqlService<L, R>
    {
        /// <summary>
        /// Performs aggregate operations: sum, avg, min, max, count.
        /// </summary>
        int Aggregate(List<L> source, Func<L, int> selector, string operation);

        /// <summary>
        /// Returns all combinations of (L, R) from the two lists (CROSS JOIN).
        /// </summary>
        List<(L, R)> CrossJoin(List<L> left, List<R> right);

        /// <summary>
        /// Returns (L, R) pairs where keySelector returns true (INNER JOIN).
        /// </summary>
        List<(L, R)> InnerJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector);

        /// <summary>
        /// Returns all items from the left list, matched with right list if keySelector returns true;
        /// otherwise pairs with default R value (LEFT JOIN).
        /// </summary>
        List<(L, R?)> LeftJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector);

        /// <summary>
        /// Filters the list based on the given predicate (WHERE).
        /// </summary>
        List<L> Where(List<L> source, Func<L, bool> predicate);

        /// <summary>
        /// Groups the list by a specified key selector (GROUP BY).
        /// </summary>
        Dictionary<K, List<L>> GroupBy<K>(List<L> source, Func<L, K> keySelector);

        /// <summary>
        /// Projects each element of the list into a new form (SELECT).
        /// </summary>
        List<T> Select<T>(List<L> source, Func<L, T> selector);

        /// <summary>
        /// Returns a list with duplicate elements removed (DISTINCT).
        /// </summary>
        List<L> Distinct(List<L> source);

        /// <summary>
        /// Sorts the list in ascending order by a key (ORDER BY ASC).
        /// </summary>
        List<L> OrderBy(List<L> source, Func<L, int> keySelector);

        /// <summary>
        /// Sorts the list in descending order by a key (ORDER BY DESC).
        /// </summary>
        List<L> OrderByDescending(List<L> source, Func<L, int> keySelector);

        /// <summary>
        /// Returns true if at least one element satisfies the predicate (EXISTS).
        /// </summary>
        bool Exists(List<L> source, Func<L, bool> predicate);

        /// <summary>
        /// Returns the first N elements from the list (TAKE / LIMIT).
        /// </summary>
        List<L> Take(List<L> source, int count);

        /// <summary>
        /// Skips the first N elements of the list (SKIP / OFFSET).
        /// </summary>
        List<L> Skip(List<L> source, int count);
    }
}
