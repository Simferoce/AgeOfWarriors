using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public T GetValueOrThrow(Entity entity)
        {
            return TryGetValue(entity, out T value) ? value : throw new Exception($"Could not resolve the path \"{path}\" for \"{entity}\"");
        }

        public T GetValueOrDefault(Entity entity)
        {
            return TryGetValue(entity, out T value) ? value : default;
        }

        private bool TryGetValue(Entity entity, out T value)
        {
            if (string.IsNullOrEmpty(path))
            {
                value = default(T);
                return false;
            }

            if (entity.StatisticRegistry.TryGetStatistic(path, out Statistic<T> statistic))
            {
                value = statistic.GetValue();
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
