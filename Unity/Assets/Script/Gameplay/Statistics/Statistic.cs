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
        protected StatisticRepository owner;

        protected Statistic(string name, StatisticDefinition definition, Value baseValue)
        {
            this.name = name;
            this.definition = definition;
            this.baseValue = baseValue;
        }

        public virtual void Initialize(StatisticRepository owner)
        {
            this.owner = owner;
            baseValue.Initialize(owner);
        }

        public abstract T Get<T>();
        public abstract T GetBase<T>();
        public abstract void Set<T>(T value);
        public abstract void SetEquation<T>(Func<T, T> modifier);

        public static implicit operator float(Statistic statistic) => statistic.Get<float>();
        public static implicit operator bool(Statistic statistic) => statistic.Get<bool>();
    }


    [Serializable]
    public abstract class Statistic<ReferenceType> : Statistic
    {
        protected Func<ReferenceType, ReferenceType> equation;

        public Statistic(string name, StatisticDefinition definition, Value baseValue, Func<ReferenceType, ReferenceType> equation = null) : base(name, definition, baseValue)
        {
            this.equation = equation;
        }

        public override T GetBase<T>()
        {
            return baseValue.GetValue<T>();
        }

        public override T Get<T>()
        {
            return StatisticUtility.ConvertGeneric<T, ReferenceType>(equation != null ? equation(baseValue.GetValue<ReferenceType>()) : baseValue.GetValue<ReferenceType>());
        }

        public override void SetEquation<T>(Func<T, T> equation)
        {
            this.equation = (Func<ReferenceType, ReferenceType>)(object)equation;
        }

        public override void Set<T>(T value)
        {
            baseValue = new SerializeValue<T>(value);
        }
    }

    [Serializable]
    public class StatisticFloat : Statistic<float>
    {
        public StatisticFloat() : base("", null, new SerializeValueFloat(0f), null)
        {
        }
        public StatisticFloat(string name, StatisticDefinition definition, float baseValue, Func<float, float> equation = null) : base(name, definition, new SerializeValueFloat(baseValue), equation)
        {
        }

        public StatisticFloat(string name, StatisticDefinition definition, Value baseValue, Func<float, float> equation = null) : base(name, definition, baseValue, equation)
        {
        }
    }
}
