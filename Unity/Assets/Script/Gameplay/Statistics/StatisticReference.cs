using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticReference
    {
        [SerializeField] private string name;

        private Entity entity;

        public string Name { get => name; set => name = value; }

        public void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public Statistic GetOrThrow()
        {
            return entity.GetCachedComponent<StatisticRepository>().GetOrThrow(name);
        }

        public Statistic Get()
        {
            return entity.GetCachedComponent<StatisticRepository>().Get(name);
        }

        public bool IsSet()
        {
            return !string.IsNullOrEmpty(name);
        }
    }

    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string name;

        private Entity entity;

        public void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public Statistic<T> GetOrThrow()
        {
            return entity.GetCachedComponent<StatisticRepository>().GetOrThrow<T>(name);
        }

        public Statistic<T> Get()
        {
            return entity.GetCachedComponent<StatisticRepository>().Get<T>(name);
        }

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
