using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    public class StatisticRepository : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics = new List<Statistic>();

        public IEnumerable<Statistic> Statistics => statistics.Concat(extensions.SelectMany(x => x.statistics));

        private Entity entity;
        private List<StatisticRepository> extensions = new List<StatisticRepository>();

        private void Awake()
        {
            entity = GetComponentInParent<Entity>();
        }

        public void Initialize()
        {
            foreach (Statistic statistic in statistics)
                statistic.Initialize(entity);
        }

        public void Add(Statistic statistic)
        {
            statistics.Add(statistic);
        }

        public void AddExtension(StatisticRepository extension)
        {
            extensions.Add(extension);
        }

        public void RemoveExtension(StatisticRepository extension)
        {
            extensions.Remove(extension);
        }

        public void Remove(Statistic statistic)
        {
            statistics.Remove(statistic);
        }

        public bool TryGet(StatisticDefinition definition, out Statistic statistic)
        {
            statistic = Statistics.FirstOrDefault(x => x.Definition == definition);
            return statistic != null;
        }

        public bool TryGet(string name, out Statistic statistic)
        {
            statistic = Statistics.FirstOrDefault(x => x.Name == name);
            return statistic != null;
        }

        public Statistic Get(StatisticDefinition definition)
        {
            return Statistics.FirstOrDefault(x => x.Definition == definition);
        }

        public Statistic Get(string name)
        {
            return Statistics.FirstOrDefault(x => x.Name == name);
        }

        public Statistic GetOrThrow(StatisticDefinition definition)
        {
            return TryGet(definition, out Statistic statistic) ? statistic : throw new System.Exception($"There is no statistic of definition \"{definition}\" in {this}");
        }

        public Statistic GetOrThrow(string name)
        {
            return TryGet(name, out Statistic statistic) ? statistic : throw new System.Exception($"There is no statistic with name \"{name}\" in {this}");
        }
    }
}
