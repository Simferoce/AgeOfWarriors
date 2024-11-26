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
        public Entity Entity { get => entity; set => entity = value; }

        protected Entity entity;

        public virtual void Initialize(Entity entity)
        {
            if (this.entity != null)
                return;

            this.entity = entity;
        }

        public abstract T GetModifiedValue<T>(Context context);
        public abstract T GetBaseValue<T>(Context context);
        public abstract Statistic Snapshot(Context context);
        public abstract bool TryGetDescription(out string description, Context context);
        public abstract string GetFormattedValue(string format, Context context);
    }

    [Serializable]
    public abstract class Statistic<ReferenceType> : Statistic
    {
        [SerializeField] protected StatisticDefinition<ReferenceType> definition;

        public override StatisticDefinition Definition { get => definition; set => definition = value as StatisticDefinition<ReferenceType>; }

        public abstract ReferenceType GetModifiedValue(Context context);
        public abstract ReferenceType GetBaseValue(Context context);

        public override T GetModifiedValue<T>(Context context)
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetModifiedValue(context));
        }

        public override T GetBaseValue<T>(Context context)
        {
            return StatisticConverter.ConvertGeneric<T, ReferenceType>(GetBaseValue(context));
        }
    }
}
