using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;

        public string Name { get => name; set => name = value; }
        public abstract StatisticDefinition Definition { get; set; }

        protected Entity entity;

        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public abstract T GetModifiedValue<T>();
        public abstract T GetBaseValue<T>();
        public abstract Statistic Snapshot();
    }

    [Serializable]
    public abstract class Statistic<ReferenceType> : Statistic
    {
        [SerializeField] protected StatisticDefinition<ReferenceType> definition;

        public override StatisticDefinition Definition { get => definition; set => definition = value as StatisticDefinition<ReferenceType>; }

        public abstract ReferenceType GetModifiedValue();
        public abstract ReferenceType GetBaseValue();

        public override T GetModifiedValue<T>()
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetModifiedValue());
        }

        public override T GetBaseValue<T>()
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetBaseValue());
        }
    }
}
