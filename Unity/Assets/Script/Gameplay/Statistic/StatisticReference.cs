using System;
using Unity.Profiling;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        static readonly ProfilerMarker resolveMarker = new ProfilerMarker("Statistic.Resolve");

        [SerializeField] private string path;

        public T GetValueOrThrow(IStatisticProvider statisticProvider)
        {
            return TryGetValue(statisticProvider, out T value) ? value : throw new Exception($"Could not resolve the path \"{path}\" for \"{statisticProvider}\"");
        }

        public T GetValueOrDefault(IStatisticProvider statisticProvider)
        {
            return TryGetValue(statisticProvider, out T value) ? value : default;
        }

        private bool TryGetValue(IStatisticProvider statisticProvider, out T value)
        {
            using (resolveMarker.Auto())
            {
                if (string.IsNullOrEmpty(path))
                {
                    value = default(T);
                    return false;
                }

                return statisticProvider.TryGetStatistic<T>(path, out value);
            }
        }
    }
}
