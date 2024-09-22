using System;

namespace Game
{
    public static class StatisticUtility
    {
        /// <summary>
        /// Convert generic. Support conversion between double/float/int/boolean/string and same type.
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <typeparam name="U">Parameter Type</typeparam>
        /// <param name="value">Parameter Value</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException"></exception>
        public static T ConvertGeneric<T, U>(U value)
        {
            if (typeof(T) == typeof(U))
            {
                U a = value;
                return __refvalue(__makeref(a), T);
            }
            else if (typeof(T) == typeof(double))
            {
                if (typeof(U) == typeof(int))
                {
                    U a = value;
                    double b = System.Convert.ToDouble(__refvalue(__makeref(a), int));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(float))
                {
                    U a = value;
                    double b = System.Convert.ToDouble(__refvalue(__makeref(a), float));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(bool))
                {
                    U a = value;
                    double b = System.Convert.ToDouble(__refvalue(__makeref(a), bool));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(string))
                {
                    U a = value;
                    double b = System.Convert.ToDouble(__refvalue(__makeref(a), string));
                    return __refvalue(__makeref(b), T);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (typeof(U) == typeof(float))
                {
                    U a = value;
                    int b = System.Convert.ToInt32(__refvalue(__makeref(a), float));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(double))
                {
                    U a = value;
                    int b = System.Convert.ToInt32(__refvalue(__makeref(a), double));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(bool))
                {
                    U a = value;
                    int b = System.Convert.ToInt32(__refvalue(__makeref(a), bool));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(string))
                {
                    U a = value;
                    int b = System.Convert.ToInt32(__refvalue(__makeref(a), string));
                    return __refvalue(__makeref(b), T);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (typeof(U) == typeof(int))
                {
                    U a = value;
                    float b = System.Convert.ToSingle(__refvalue(__makeref(a), int));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(double))
                {
                    U a = value;
                    float b = System.Convert.ToSingle(__refvalue(__makeref(a), double));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(bool))
                {
                    U a = value;
                    float b = System.Convert.ToSingle(__refvalue(__makeref(a), bool));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(string))
                {
                    U a = value;
                    float b = System.Convert.ToSingle(__refvalue(__makeref(a), string));
                    return __refvalue(__makeref(b), T);
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                if (typeof(U) == typeof(int))
                {
                    U a = value;
                    bool b = System.Convert.ToBoolean(__refvalue(__makeref(a), int));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(float))
                {
                    U a = value;
                    bool b = System.Convert.ToBoolean(__refvalue(__makeref(a), float));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(double))
                {
                    U a = value;
                    bool b = System.Convert.ToBoolean(__refvalue(__makeref(a), double));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(string))
                {
                    U a = value;
                    bool b = System.Convert.ToBoolean(__refvalue(__makeref(a), string));
                    return __refvalue(__makeref(b), T);
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (typeof(U) == typeof(int))
                {
                    U a = value;
                    string b = System.Convert.ToString(__refvalue(__makeref(a), int));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(float))
                {
                    U a = value;
                    string b = System.Convert.ToString(__refvalue(__makeref(a), float));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(double))
                {
                    U a = value;
                    string b = System.Convert.ToString(__refvalue(__makeref(a), double));
                    return __refvalue(__makeref(b), T);
                }
                else if (typeof(U) == typeof(bool))
                {
                    U a = value;
                    string b = System.Convert.ToString(__refvalue(__makeref(a), bool));
                    return __refvalue(__makeref(b), T);
                }
            }

            throw new System.InvalidCastException($"Could not convert from {typeof(U).Name} to {typeof(T).Name}.");
        }

        public static Statistic Resolve(IStatisticContext context, string path)
        {
            ReadOnlySpan<char> span = path.AsSpan();
            int start = 0;
            int index;

            IStatisticContext current = context;
            while ((index = span.Slice(start).IndexOf(".")) != -1)
            {
                if (!TryRetreiveStatistic<IStatisticContext>(current, span.Slice(start, index - start), out current))
                    return null;

                start += index + 1;
            }

            if (!TryRetreiveStatistic<Statistic>(current, span.Slice(start), out Statistic value))
                return null;

            return value;
        }

        private static bool TryRetreiveStatistic<T>(IStatisticContext current, ReadOnlySpan<char> span, out T value)
        {
            foreach (Statistic statistic in current.GetStatistic())
            {
                if (span.SequenceEqual(statistic.Name))
                {
                    value = statistic.GetValueOrThrow<T>();
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
