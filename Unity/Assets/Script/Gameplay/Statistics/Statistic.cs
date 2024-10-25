using System;
using System.Linq;
using UnityEngine;
using static Game.Statistics.StatisticDefinition;

namespace Game.Statistics
{
    [Serializable]
    public abstract class Statistic
    {
        [SerializeField] protected string name;
        [SerializeField] protected StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
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
        public abstract T GetValue(Context context);

        public override ReferenceType GetValue<ReferenceType>(Context context)
        {
            return StatisticConverter.ConvertGeneric<ReferenceType, T>(GetValue(context));
        }
    }

    [Serializable]
    public class StatisticStandardFloat : Statistic<float>
    {
        [SerializeReference, SubclassSelector] protected StatisticValue value;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            value.Initialize(owner);
        }

        public override float GetValue(Context context)
        {
            float baseValue = (value as StatisticValue<float>).GetValue(context);
            float flatValue = definition.Modifiers.Where(x => x.Operation == OperationType.Flat).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).Sum();
            float percentageValue = definition.Modifiers.Where(x => x.Operation == OperationType.Percentage).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).Sum();
            float multiplierValue = definition.Modifiers.Where(x => x.Operation == OperationType.Multiplier).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).Aggregate(1f, (x, y) => x * y);
            float maximumValue = definition.Modifiers.Where(x => x.Operation == OperationType.Maximum).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).DefaultIfEmpty(float.MaxValue).Min();
            float minimumValue = definition.Modifiers.Where(x => x.Operation == OperationType.Maximum).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).DefaultIfEmpty(float.MinValue).Max();

            return Mathf.Clamp((baseValue + flatValue) * (1 + percentageValue) * multiplierValue, minimumValue, maximumValue);
        }
    }

    [Serializable]
    public abstract class StatisticModifiable<T> : Statistic<T>
    {
        [SerializeReference, SubclassSelector] protected StatisticValue defaultValue;

        protected T currentValue;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            defaultValue.Initialize(owner);
            currentValue = (defaultValue as StatisticValue<T>).GetValue(null);
        }

        public abstract void Modify(T value, Context context);

        public override T GetValue(Context context)
        {
            return currentValue;
        }
    }

    [Serializable]
    public class StatisticModifiableFloat : StatisticModifiable<float>
    {
        public override void Modify(float value, Context context)
        {
            float maximumValue = definition.Modifiers.Where(x => x.Operation == OperationType.Maximum).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).DefaultIfEmpty(float.MaxValue).Min();
            float minimumValue = definition.Modifiers.Where(x => x.Operation == OperationType.Maximum).SelectMany(x => owner.GetCachedComponent<StatisticIndex>().Statistics.Where(y => y.Definition == x).Select(y => (y as Statistic<float>).GetValue(context))).DefaultIfEmpty(float.MinValue).Max();
            value = Mathf.Clamp(value, minimumValue, maximumValue);
            currentValue = value;
        }
    }
}