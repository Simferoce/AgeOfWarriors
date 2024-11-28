using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StatisticRegistry
    {
        private List<Statistic> statistics;

        public void Register(Statistic statistic)
        {
            statistics.Add(statistic);
        }

        public void Unregister(Statistic statistic)
        {
            statistics.Remove(statistic);
        }

        public bool TryGetStatistic<T>(string name, out Statistic<T> statistic)
        {
            foreach (Statistic registryStatistic in statistics)
            {
                if (registryStatistic.Name == name)
                {
                    if (registryStatistic is Statistic<T> covnertStatistic)
                    {
                        statistic = covnertStatistic;
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning($"Found a statistic with the name \"{name}\" with the type \"{registryStatistic.GetType()}\" but was expecting type \"{typeof(Statistic<T>)}\".");
                        statistic = null;
                        return false;
                    }

                }
            }

            statistic = null;
            return false;
        }

        public bool TryGetStatistic<T>(StatisticDefinition definition, out Statistic<T> statistic)
        {
            foreach (Statistic registryStatistic in statistics)
            {
                if (registryStatistic.Definition == definition)
                {
                    if (registryStatistic is Statistic<T> covnertStatistic)
                    {
                        statistic = covnertStatistic;
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning($"Found a statistic with the definition \"{definition}\" with the type \"{registryStatistic.GetType()}\" but was expecting type \"{typeof(Statistic<T>)}\".");
                        statistic = null;
                        return false;
                    }

                }
            }

            statistic = null;
            return false;
        }

        public Statistic<T> GetStatisticOrThrow<T>(string name)
        {
            return TryGetStatistic<T>(name, out Statistic<T> statistic) ? statistic : throw new System.Exception($"Did not find any statistic with the name \"{name}\" and type \"{typeof(Statistic<T>)}\"");
        }

        public bool TryGetStatistic(StatisticDefinition definition, out Statistic statistic)
        {
            foreach (Statistic registryStatistic in statistics)
            {
                if (registryStatistic.Definition == definition)
                {
                    statistic = registryStatistic;
                    return true;
                }
            }

            statistic = null;
            return false;
        }

        public Statistic GetStatisticOrThrow(StatisticDefinition definition)
        {
            return TryGetStatistic(definition, out Statistic statistic) ? statistic : throw new System.Exception($"Did not find any statistic with the definition \"{definition}\".");
        }

    }
}
