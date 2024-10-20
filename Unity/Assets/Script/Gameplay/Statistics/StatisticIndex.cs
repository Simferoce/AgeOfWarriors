using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Game.Statistics
{
    public class StatisticIndex
    {
        public IReadOnlyList<StatisticIndex> Relations => relations;
        public IReadOnlyList<Statistic> Statistics => statistics;

        private List<StatisticIndex> relations = new List<StatisticIndex>();
        private List<Statistic> statistics = new List<Statistic>();
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
                    result += statistic.GetValue<float>(context);
                }
            }

            foreach (StatisticIndex relation in relations)
                result *= relation.SumByDefinition(definition);

            return result;
        }
    }
}
