﻿namespace Game.Statistics
{
    public static class StatisticConverter
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
            else if (typeof(T).IsAssignableFrom(typeof(U)))
            {
                return (T)(object)value;
            }

            throw new System.InvalidCastException($"Could not convert from {typeof(U).Name} to {typeof(T).Name}.");
        }
    }
}
