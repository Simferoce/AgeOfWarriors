using System;
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
            float maximumValue = owner.GetCachedComponent<StatisticRegistry>().Maximum(definition.Maximum, context);
            float minimumValue = owner.GetCachedComponent<StatisticRegistry>().Minimum(definition.Minimum, context);
            value = Mathf.Clamp(value, minimumValue, maximumValue);
            currentValue = value;
        }

        protected override float GetValue(Context context)
        {
            return currentValue;
        }

        public override string GetDescription(Context context)
        {
            return $"<color=#{definition.ColorHex}>({currentValue})</color>";
        }
    }
}
