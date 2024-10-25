using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticIndex
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics = new List<Statistic>();

        public IReadOnlyList<StatisticIndex> Relations => relations;
        public IReadOnlyList<Statistic> Statistics => statistics;

        public Entity Entity { get => entity; set => entity = value; }

        private List<StatisticIndex> relations = new List<StatisticIndex>();
        private Entity entity;

        public void Initialize(Entity entity)
        {
            this.entity = entity;

            foreach (Statistic statistic in statistics.Where(x => x != null))
                statistic.Initialize(entity);
        }

        public void Add(Statistic statistic)
        {
            statistic.Initialize(entity);
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

        public Statistic Get(StatisticIdentifiant identifiant)
        {
            return statistics.FirstOrDefault(x => x.Definition == StatisticDefinitionRepository.Instance.GetById(identifiant));
        }
    }
}
