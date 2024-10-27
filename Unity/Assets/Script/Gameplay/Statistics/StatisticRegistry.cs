using Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Statistics
{
    public class StatisticRegistry
    {
        private List<Statistic> statistics = new List<Statistic>();

        public Entity Entity { get => entity; set => entity = value; }

        private List<StatisticRegistry> relations = new List<StatisticRegistry>();
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

        public void Add(StatisticRegistry statisticRegistry)
        {
            relations.Add(statisticRegistry);
        }

        public void Remove(StatisticRegistry statisticRegistry)
        {
            relations.Remove(statisticRegistry);
        }

        public void Modify<T>(T value, StatisticIdentifiant identifiant, Context context = null)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == StatisticDefinitionRegistry.Instance.GetById(identifiant));
            Assert.IsNotNull(statistic, $"Unable to modify the statistic \"{identifiant}\" because it does not exists in the index of {Entity.transform.GetFullPath()}");
            if (statistic is not IStatisticModifiable<T> modifiableStatistic)
                throw new Exception($"Unable to modify the statistic \"{identifiant}\" in \"{Entity.transform.GetFullPath()}\" because it is not of type \"{nameof(IStatisticModifiable<T>)}\".");

            modifiableStatistic.Modify(value, context);
        }

        public T GetOrThrow<T>(StatisticIdentifiant identifiant, Context context = null)
        {
            return GetOrThrow<T>(StatisticDefinitionRegistry.Instance.GetById(identifiant), context);
        }

        public T GetOrThrow<T>(StatisticDefinition definition, Context context = null)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == definition);
            Assert.IsNotNull(statistic, $"Unable to get the statistic with the definition {definition} from {entity.transform.GetFullPath()}.");
            return statistic.GetValue<T>(context);
        }

        public T GetOrDefault<T>(StatisticIdentifiant identifiant, T defaultValue, Context context = null)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == StatisticDefinitionRegistry.Instance.GetById(identifiant));
            if (statistic == null)
                return defaultValue;

            return statistic.GetValue<T>(context);
        }

        public bool TryGetStatistic(string name, out Statistic value)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Name == name);
            if (statistic == null)
            {
                value = default;
                return false;
            }

            value = statistic;
            return true;
        }

        public bool TryGetStatistic<T>(string name, out Statistic<T> value)
        {
            Statistic<T> statistic = statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Name == name);
            if (statistic == null)
            {
                value = default;
                return false;
            }

            value = statistic;
            return true;
        }

        public bool TryGet<T>(StatisticIdentifiant identifiant, out T value, Context context = null)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Definition == StatisticDefinitionRegistry.Instance.GetById(identifiant));
            if (statistic == null)
            {
                value = default;
                return false;
            }

            value = statistic.GetValue<T>(context);
            return true;
        }

        public float Sum(StatisticIdentifiant identifiant, Context context = null)
        {
            return statistics.Where(x => x.Definition == StatisticDefinitionRegistry.Instance.GetById(identifiant))
                             .Select(x => x.GetValue<float>(context))
                             .Sum()
                   + relations.Sum(x => x.Sum(identifiant, context));
        }

        public float Sum(IEnumerable<StatisticIdentifiant> identifiants, Context context = null)
        {
            return Sum(identifiants.Select(x => StatisticDefinitionRegistry.Instance.GetById(x)), context);
        }

        public float Sum(IEnumerable<StatisticDefinition> identifiants, Context context = null)
        {
            return statistics.Where(x => identifiants.Contains(x.Definition))
                             .Select(x => x.GetValue<float>(context))
                             .Sum()
                   + relations.Sum(x => x.Sum(identifiants, context));
        }

        public float Multiply(StatisticIdentifiant identifiant, Context context = null)
        {
            return statistics.Where(x => x.Definition == StatisticDefinitionRegistry.Instance.GetById(identifiant))
                             .Select(x => x.GetValue<float>(context))
                             .Aggregate(1f, (x, y) => x * y)
                   * relations.Aggregate(1f, (x, y) => x * y.Multiply(identifiant, context));
        }

        public float Multiply(IEnumerable<StatisticIdentifiant> identifiants, Context context = null)
        {
            return Multiply(identifiants.Select(x => StatisticDefinitionRegistry.Instance.GetById(x)), context);
        }

        public float Multiply(IEnumerable<StatisticDefinition> identifiants, Context context = null)
        {
            return statistics.Where(x => identifiants.Contains(x.Definition))
                             .Select(x => x.GetValue<float>(context))
                             .Aggregate(1f, (x, y) => x * y)
                   * relations.Aggregate(1f, (x, y) => x * y.Multiply(identifiants, context));
        }

        public float Maximum(StatisticDefinition identifiant, Context context = null)
        {
            return Mathf.Min(statistics.Where(x => x.Definition == identifiant)
                                        .Select(y => y.GetValue<float>(context))
                                        .DefaultIfEmpty(float.MaxValue)
                                        .Min(),
                             relations.Select(x => x.Maximum(identifiant, context)).DefaultIfEmpty(float.MaxValue).Min());
        }

        public float Maximum(StatisticIdentifiant identifiant, Context context = null)
        {
            return Maximum(StatisticDefinitionRegistry.Instance.GetById(identifiant), context);
        }

        public float Maximum(IEnumerable<StatisticIdentifiant> identifiants, Context context = null)
        {
            return Maximum(identifiants.Select(x => StatisticDefinitionRegistry.Instance.GetById(x)), context);
        }

        public float Maximum(IEnumerable<StatisticDefinition> identifiants, Context context = null)
        {
            return Mathf.Min(statistics.Where(x => identifiants.Contains(x.Definition))
                                        .Select(y => y.GetValue<float>(context))
                                        .DefaultIfEmpty(float.MaxValue)
                                        .Min(),
                             relations.Select(x => x.Maximum(identifiants, context)).DefaultIfEmpty(float.MaxValue).Min());
        }

        public float Minimum(StatisticIdentifiant identifiant, Context context = null)
        {
            return Minimum(StatisticDefinitionRegistry.Instance.GetById(identifiant), context);
        }

        public float Minimum(StatisticDefinition identifiant, Context context = null)
        {
            return Mathf.Min(statistics.Where(x => x.Definition == identifiant)
                                        .Select(y => y.GetValue<float>(context))
                                        .DefaultIfEmpty(float.MinValue)
                                        .Max(),
                             relations.Select(x => x.Minimum(identifiant, context)).DefaultIfEmpty(float.MinValue).Max());
        }

        public float Minimum(IEnumerable<StatisticIdentifiant> identifiants, Context context = null)
        {
            return Minimum(identifiants.Select(x => StatisticDefinitionRegistry.Instance.GetById(x)), context);
        }

        public float Minimum(IEnumerable<StatisticDefinition> identifiants, Context context = null)
        {
            return Mathf.Min(statistics.Where(x => identifiants.Contains(x.Definition))
                                        .Select(y => y.GetValue<float>(context))
                                        .DefaultIfEmpty(float.MinValue)
                                        .Max(),
                             relations.Select(x => x.Minimum(identifiants, context)).DefaultIfEmpty(float.MinValue).Max());
        }

    }
}
