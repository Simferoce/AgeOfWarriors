using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticReferenceValue
    {
        [SerializeField] private string name;

        private Entity entity;

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
    }
}
