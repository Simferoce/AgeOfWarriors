using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticIndex
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics = new List<Statistic>();

        public IReadOnlyList<StatisticIndex> Relations => relations;
        public IReadOnlyList<Statistic> Statistics => statistics;

        private List<StatisticIndex> relations = new List<StatisticIndex>();
        private object context;

        public void Initialize(object context)
        {
            this.context = context;
        }

        public void Add(Statistic statistic)
        {
            statistics.Add(statistic);
        }

        public void Remove(Statistic statistic)
        {
            statistics.Remove(statistic);
        }

        public void Add(StatisticIndex statisticIndex)
        {
            relations.Add(statisticIndex);
        }

        public void Remove(StatisticIndex statisticIndex)
        {
            relations.Remove(statisticIndex);
        }

        public T SelfByDefinition<T>(StatisticIdentifiant statisticIdentifiant)
        {
            return SelfByDefinition<T>(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant));
        }

        public bool TrySelfByDefinition<T>(StatisticIdentifiant statisticIdentifiant, out T value)
        {
            return TrySelfByDefinition<T>(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant), out value);
        }

        public bool TrySelfByDefinition<T>(StatisticDefinition definition, out T value)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == definition);
            value = statistic != null ? statistic.GetValue<T>(context) : default;
            return statistic != null;
        }

        public T SelfByDefinition<T>(StatisticDefinition definition)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == definition);
            Assert.IsNotNull(statistic, $"Unable to get the statistic \"{definition}\" from \"{context}\"");

            return statistic.GetValue<T>(context);
        }

        public float SumByDefinition(StatisticIdentifiant statisticIdentifiant)
        {
            return SumByDefinition(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant));
        }

        public float SumByDefinition(StatisticDefinition definition)
        {
            float result = 0f;
            foreach (Statistic statistic in statistics)
            {
                if (statistic.Definition == definition)
                {
                    result += statistic.GetValue<float>(context);
                }
            }

            foreach (StatisticIndex relation in relations)
                result += relation.SumByDefinition(definition);

            return result;
        }

        public float MultiplierByDefinition(StatisticIdentifiant statisticIdentifiant)
        {
            return MultiplierByDefinition(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant));
        }

        public float MultiplierByDefinition(StatisticDefinition definition)
        {
            float result = 1f;
            foreach (Statistic statistic in statistics)
            {
                if (statistic.Definition == definition)
                {
                    result *= statistic.GetValue<float>(context);
                }
            }

            foreach (StatisticIndex relation in relations)
                result *= relation.MultiplierByDefinition(definition);

            return result;
        }

        public bool Any(StatisticDefinition definition)
        {
            return statistics.Any(x => x.Definition == definition && x.GetValue<bool>(context)) || relations.Any(x => x.Any(definition));
        }

        public bool Any(StatisticIdentifiant statisticIdentifiant)
        {
            return Any(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant));
        }

        public float Max(StatisticDefinition definition)
        {
            float max = statistics.Where(x => x.Definition == definition).DefaultIfEmpty().Max(x => x?.GetValue<float>(context) ?? 0f);
            foreach (StatisticIndex relation in relations)
                max = Mathf.Max(max, relation.Max(definition));

            return max;
        }

        public float Max(StatisticIdentifiant statisticIdentifiant)
        {
            return Max(StatisticDefinitionRepository.Instance.GetById(statisticIdentifiant));
        }

        public bool TryGetStatisticByName(string name, out Statistic statistic)
        {
            statistic = statistics.FirstOrDefault(x => x.Name == name);
            if (statistic != null)
                return true;

            foreach (StatisticIndex relation in relations)
            {
                if (relation.TryGetStatisticByName(name, out statistic))
                    return true;
            }

            return false;
        }
    }
}
