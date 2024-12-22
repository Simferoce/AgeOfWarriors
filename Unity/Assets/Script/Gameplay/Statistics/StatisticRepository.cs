using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticRepository
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics = new List<Statistic>();

        public Statistic this[StatisticDefinition definition] => TryGet(definition, out Statistic statistic) ? statistic : throw new System.Exception($"Statistic with definition \"{definition}\" not found in {this.Owner}.");

        public object Owner { get; private set; }

        public void Initialize(object owner)
        {
            this.Owner = owner;
            foreach (Statistic statistic in statistics)
                statistic.Initialize(this);
        }

        public void Add(Statistic statistic)
        {
            statistic.Initialize(this);
            statistics.Add(statistic);
        }

        public void Remove(Statistic statistic)
        {
            statistics.Remove(statistic);
        }

        public bool TryGet<T>(StatisticDefinition definition, out Statistic<T> statistic)
        {
            statistic = statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Definition == definition);
            return statistic != null;
        }

        public bool TryGet<T>(string name, out Statistic<T> statistic)
        {
            statistic = statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Name == name);
            return statistic != null;
        }

        public IEnumerable<Statistic<T>> GetAll<T>(StatisticDefinition definition)
        {
            return statistics.OfType<Statistic<T>>().Where(x => x.Definition == definition);
        }

        public IEnumerable<Statistic<T>> GetAll<T>()
        {
            return statistics.OfType<Statistic<T>>();
        }

        public Statistic<T> Get<T>(StatisticDefinition definition)
        {
            return statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Definition == definition);
        }

        public Statistic<T> Get<T>(string name)
        {
            return statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Name == name);
        }

        public Statistic<T> GetOrThrow<T>(StatisticDefinition definition)
        {
            return TryGet(definition, out Statistic<T> statistic) ? statistic : throw new System.Exception($"There is no statistic of definition \"{definition}\" in {this}");
        }

        public Statistic<T> GetOrThrow<T>(string name)
        {
            return TryGet(name, out Statistic<T> statistic) ? statistic : throw new System.Exception($"There is no statistic with name \"{name}\" in {this}");
        }

        public bool TryGet(StatisticDefinition definition, out Statistic statistic)
        {
            statistic = statistics.FirstOrDefault(x => x.Definition == definition);
            return statistic != null;
        }

        public bool TryGet(string name, out Statistic statistic)
        {
            statistic = statistics.FirstOrDefault(x => x.Name == name);
            return statistic != null;
        }

        public Statistic Get(StatisticDefinition definition)
        {
            return statistics.FirstOrDefault(x => x.Definition == definition);
        }

        public Statistic Get(string name)
        {
            return statistics.FirstOrDefault(x => x.Name == name);
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
