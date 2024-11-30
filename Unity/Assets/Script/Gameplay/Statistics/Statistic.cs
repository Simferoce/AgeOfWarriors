using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;
        [SerializeField] protected StatisticDefinition definition;
        [SerializeReference, SubclassSelector] protected Value baseValue;

        public string Name { get => name; set => name = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public Entity Entity { get => entity; set => entity = value; }

        protected Entity entity;

        protected Statistic(string name, StatisticDefinition definition, Value baseValue)
        {
            this.name = name;
            this.definition = definition;
            this.baseValue = baseValue;
        }

        public virtual void Initialize(Entity entity)
        {
            if (this.entity != null)
                return;

            this.entity = entity;
            baseValue.Initialize(entity);
        }

        public abstract T Get<T>();
        public abstract T GetBase<T>();
        public abstract void Set<T>(T value);

        public static implicit operator float(Statistic statistic) => statistic.Get<float>();
        public static implicit operator bool(Statistic statistic) => statistic.Get<bool>();
    }


    [Serializable]
    public abstract class Statistic<ReferenceType> : Statistic
    {
        protected ReferenceType currentValue;

        public Statistic(string name, StatisticDefinition definition, Value baseValue) : base(name, definition, baseValue)
        {
        }

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            currentValue = baseValue.GetValue<ReferenceType>();
        }

        public override T GetBase<T>()
        {
            return baseValue.GetValue<T>();
        }

        public override T Get<T>()
        {
            return StatisticUtility.ConvertGeneric<T, ReferenceType>(currentValue);
        }

        public override void Set<T>(T value)
        {
            currentValue = StatisticUtility.ConvertGeneric<ReferenceType, T>(value);
        }
    }

    [Serializable]
    public class StatisticFloat : Statistic<float>
    {
        public StatisticFloat() : base("", null, new SerializeValueFloat(0f))
        {
        }

        public StatisticFloat(string name, StatisticDefinition definition, float baseValue) : base(name, definition, new SerializeValueFloat(baseValue))
        {
        }

        public StatisticFloat(string name, StatisticDefinition definition, Value baseValue) : base(name, definition, baseValue)
        {
        }
    }
}
