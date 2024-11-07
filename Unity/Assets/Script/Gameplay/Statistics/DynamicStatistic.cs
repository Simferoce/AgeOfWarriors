using System;

namespace Game.Statistics
{
    [Serializable]
    public class DynamicStatistic : Statistic
    {
        private float current;

        public void Set(float value)
        {
            current = value;
        }

        public override float GetBaseValue()
        {
            return current;
        }

        public override float GetModifiedValue()
        {
            return definition.Modify(current, entity.GetCachedComponent<StatisticRepository>());
        }
    }
}
