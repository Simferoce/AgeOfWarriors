using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;

        public abstract StatisticDefinition Definition { get; set; }
        public string Name { get => name; set => name = value; }
        protected Entity owner;

        public virtual void Initialize(Entity owner)
        {
            this.owner = owner;
        }

        public abstract T GetValue<T>(Context context);
    }

    [Serializable]
    public abstract class Statistic<T> : Statistic
    {
        protected abstract T GetValue(Context context);

        public override ReferenceType GetValue<ReferenceType>(Context context)
        {
            return StatisticConverter.ConvertGeneric<ReferenceType, T>(GetValue(context));
        }
    }

    [Serializable]
    public abstract class Statistic<T, U> : Statistic<T>
        where U : StatisticDefinition
    {
        [SerializeField] protected U definition;

        public override StatisticDefinition Definition { get => definition; set => definition = (U)value; }
    }
}