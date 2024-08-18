using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public T GetValueOrDefault(IEnumerable<IContext> context)
        {
            return TryGetValue(context, out T value) == true ? value : default(T);
        }

        public T GetValueOrThrow(IEnumerable<IContext> context)
        {
            if (TryGetValue(context, out T value))
                return value;

            throw new Exception($"Could not resolve the statistic of {path} for {context}");
        }

        public T GetValueOrThrow(IContext context)
        {
            return GetValueOrThrow(new List<IContext>() { context });
        }

        public T GetValueOrDefault(IContext context)
        {
            return GetValueOrDefault(new List<IContext>() { context });
        }

        private bool TryGetValue(IEnumerable<IContext> context, out T value)
        {
            if (string.IsNullOrEmpty(path))
            {
                value = default(T);
                return false;
            }

            object result = StatisticResolverService.Resolve(context, path);
            if (result is not T)
            {
                value = default;
                return false;
            }

            value = (T)result;
            return true;
        }
    }
}
