using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public bool TryGetValue(Context context, out T value)
        {
            if (string.IsNullOrEmpty(path))
            {
                value = default(T);
                return false;
            }

            object result = Resolve(context, path);
            if (result is not T)
            {
                value = default;
                return false;
            }

            value = (T)result;
            return true;
        }

        public object Resolve(object current, string path)
        {
            PropertyInfo[] propertyInfos = current.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                StatisticAttribute statisticAttribute = propertyInfo.GetCustomAttribute<StatisticAttribute>();
                if (path.StartsWith(statisticAttribute.Name))
                {
                    current = propertyInfo.GetValue(current);
                    if (path == statisticAttribute.Name)
                        return current;

                    path = path.Substring(statisticAttribute.Name.Length);
                    if (current is IDictionary children)
                    {
                        foreach (object key in children.Keys)
                        {
                            Assert.IsTrue(key is string, $"Expecting the key of the dictionnary {current.ToString()} to be a string. Remaining Path: {path}");
                            string statisticName = (string)key;
                            if (!path.StartsWith(statisticName))
                                continue;

                            path = path.Substring(statisticName.Length + 1);

                            object value = children[key];
                            object result = Resolve(value, path);
                            if (result != null)
                                return result;
                        }
                    }
                }
            }

            return null;
        }

        public T GetValueOrDefault(Context context)
        {
            return TryGetValue(context, out T value) == true ? value : default(T);
        }

        public T GetValueOrThrow(Context context)
        {
            if (TryGetValue(context, out T value))
                return value;

            throw new Exception($"Could not resolve the statistic of {value}");
        }
    }
}
