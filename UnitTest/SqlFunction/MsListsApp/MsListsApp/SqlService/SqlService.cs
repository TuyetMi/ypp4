using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.SqlService
{
    public class SqlService<L, R> : ISqlService<L, R>
    {
        public int Aggregate(List<L> source, Func<L, int> selector, string operation)
        {
            if (source == null || source.Count == 0)
                return 0;

            int result = 0;

            if (operation.Equals("sum", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < source.Count; i++)
                {
                    result += selector(source[i]);
                }
            }
            else if (operation.Equals("avg", StringComparison.OrdinalIgnoreCase))
            {
                int sum = 0;
                for (int i = 0; i < source.Count; i++)
                {
                    sum += selector(source[i]);
                }
                result = sum / source.Count;
            }
            else if (operation.Equals("max", StringComparison.OrdinalIgnoreCase))
            {
                result = selector(source[0]);
                for (int i = 1; i < source.Count; i++)
                {
                    int value = selector(source[i]);
                    if (value > result)
                        result = value;
                }
            }
            else if (operation.Equals("min", StringComparison.OrdinalIgnoreCase))
            {
                result = selector(source[0]);
                for (int i = 1; i < source.Count; i++)
                {
                    int value = selector(source[i]);
                    if (value < result)
                        result = value;
                }
            }
            else if (operation.Equals("count", StringComparison.OrdinalIgnoreCase))
            {
                result = source.Count;
            }
            else
            {
                throw new ArgumentException("Unsupported aggregate operation");
            }

            return result;
        }

        public List<(L, R)> CrossJoin(List<L> left, List<R> right)
        {
            var result = new List<(L, R)>();

            for (int i = 0; i < left.Count; i++)
            {
                for (int j = 0; j < right.Count; j++)
                {
                    result.Add((left[i], right[j]));
                }
            }

            return result;
        }

        public List<(L, R)> InnerJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R)>();

            for (int i = 0; i < left.Count; i++)
            {
                for (int j = 0; j < right.Count; j++)
                {
                    if (keySelector(left[i], right[j]))
                    {
                        result.Add((left[i], right[j]));
                    }
                }
            }

            return result;
        }

        public List<(L, R?)> LeftJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R?)>();

            for (int i = 0; i < left.Count; i++)
            {
                bool matched = false;

                for (int j = 0; j < right.Count; j++)
                {
                    if (keySelector(left[i], right[j]))
                    {
                        result.Add((left[i], right[j]));
                        matched = true;
                    }
                }

                if (!matched)
                {
                    result.Add((left[i], default(R)));
                }
            }

            return result;
        }

        public List<L> Where(List<L> source, Func<L, bool> predicate)
        {
            var result = new List<L>();

            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i]))
                {
                    result.Add(source[i]);
                }
            }

            return result;
        }
        public Dictionary<K, List<L>> GroupBy<K>(List<L> source, Func<L, K> keySelector)
        {
            var result = new Dictionary<K, List<L>>();

            for (int i = 0; i < source.Count; i++)
            {
                var key = keySelector(source[i]);
                if (!result.ContainsKey(key))
                {
                    result[key] = new List<L>();
                }
                result[key].Add(source[i]);
            }

            return result;
        }
        public List<T> Select<T>(List<L> source, Func<L, T> selector)
        {
            var result = new List<T>();

            for (int i = 0; i < source.Count; i++)
            {
                result.Add(selector(source[i]));
            }

            return result;
        }
        public List<L> Distinct(List<L> source)
        {
            var result = new List<L>();

            for (int i = 0; i < source.Count; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < result.Count; j++)
                {
                    if (EqualityComparer<L>.Default.Equals(source[i], result[j]))
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    result.Add(source[i]);
                }
            }

            return result;
        }
        public List<L> OrderBy(List<L> source, Func<L, int> keySelector)
        {
            var result = new List<L>(source);

            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    if (keySelector(result[i]) > keySelector(result[j]))
                    {
                        var temp = result[i];
                        result[i] = result[j];
                        result[j] = temp;
                    }
                }
            }

            return result;
        }

        public List<L> OrderByDescending(List<L> source, Func<L, int> keySelector)
        {
            var result = new List<L>(source);

            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    if (keySelector(result[i]) < keySelector(result[j]))
                    {
                        var temp = result[i];
                        result[i] = result[j];
                        result[j] = temp;
                    }
                }
            }

            return result;
        }
        public List<L> Take(List<L> source, int count)
        {
            var result = new List<L>();
            for (int i = 0; i < source.Count && i < count; i++)
            {
                result.Add(source[i]);
            }
            return result;
        }

        public List<L> Skip(List<L> source, int count)
        {
            var result = new List<L>();
            for (int i = count; i < source.Count; i++)
            {
                result.Add(source[i]);
            }
            return result;
        }
        public bool Exists(List<L> source, Func<L, bool> predicate)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i]))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
