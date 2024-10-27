using System;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    public interface IStatisticModifiable<T>
    {
        public void Modify(T value, Context context);
    }

    [Serializable]
    public class StatisticModifiable : Statistic<float, ModifiableStatisticDefinition>, IStatisticModifiable<float>
    {
        private float currentValue;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
        }

        public void Modify(float value, Context context)
        {
            float maximumValue = definition.Maximum.SelectMany(x => owner.GetCachedComponent<StatisticRegistry>().Statistics.Where(y => y.Definition == x).Select(y => y.GetValue<float>(context))).DefaultIfEmpty(float.MaxValue).Min();
            float minimumValue = definition.Minimum.SelectMany(x => owner.GetCachedComponent<StatisticRegistry>().Statistics.Where(y => y.Definition == x).Select(y => y.GetValue<float>(context))).DefaultIfEmpty(float.MinValue).Max();
            value = Mathf.Clamp(value, minimumValue, maximumValue);
            currentValue = value;
        }

        protected override float GetValue(Context context)
        {
            return currentValue;
        }
    }
}
