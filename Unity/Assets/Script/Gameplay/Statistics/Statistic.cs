using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;
        [SerializeField] protected StatisticDefinition definition;

        public string Name { get => name; set => name = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }

        protected Entity entity;

        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public abstract float GetModifiedValue();
        public abstract float GetBaseValue();
    }
}
