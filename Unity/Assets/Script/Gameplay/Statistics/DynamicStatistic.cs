using System;

namespace Game.Statistics
{
    [Serializable]
    public class DynamicStatistic<T> : Statistic<T>
    {
        private T current;

        public void Set(T value)
        {
            current = value;
        }

        public override Statistic Snapshot()
        {
            return new DynamicStatistic<T>() { definition = this.definition, current = GetModifiedValue() };
        }

        public override T GetBaseValue()
        {
            return current;
        }

        public override T GetModifiedValue()
        {
            return definition.Modify(current, entity.GetCachedComponent<StatisticRepository>());
        }
    }

    [Serializable]
    public class DynamicStatisticFloat : DynamicStatistic<float> { }
}
