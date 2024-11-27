using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour, IStatisticProvider
    {
        private Dictionary<Type, object> cached = new Dictionary<Type, object>();

        public float this[string name] => GetValueOrThrow<float>(name);

        public T GetValueOrThrow<T>(string name)
        {
            return TryGetStatistic<T>(name, out T statistic) ? statistic : throw new Exception($"Unable to find the value with the name: {name}");
        }

        public T GetStatisticOrDefault<T>(string name, T defaultValue = default)
        {
            return TryGetStatistic<T>(name, out T statistic) ? statistic : defaultValue;
        }

        public virtual bool TryGetStatistic<T>(ReadOnlySpan<char> name, out T statistic)
        {
            statistic = default;
            return false;
        }

        public bool TryGetCachedComponent<T>(out T component)
        {
            component = GetCachedComponent<T>();
            return component != null;
        }

        public T GetCachedComponent<T>()
        {
            if (cached.ContainsKey(typeof(T)))
            {
                return (T)cached[typeof(T)];
            }
            else
            {
                cached[typeof(T)] = GetComponent<T>();
                return (T)cached[typeof(T)];
            }
        }

        public T AddOrGetCachedComponent<T>()
            where T : Component
        {
            T component = GetCachedComponent<T>();
            if (component != null)
                return component;

            component = gameObject.AddComponent<T>();
            cached[typeof(T)] = component;

            return component;
        }
    }
}
