using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Game
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

        public T SelfByDefinition<T>(StatisticDefinition definition)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.GetDefinition(context) == definition);
            Assert.IsNotNull(statistic, $"Unable to get the statistic \"{definition}\" from \"{context}\"");

            return statistic.GetValue<T>(context);
        }

        public float SumByDefinition(StatisticDefinition definition)
        {
            float result = 0f;
            foreach (Statistic statistic in statistics)
            {
                if (statistic.GetDefinition(context) == definition)
                {
                    result += statistic.GetValue<float>(context);
                }
            }

            foreach (StatisticIndex relation in relations)
                result += relation.SumByDefinition(definition);

            return result;
        }
    }
}
