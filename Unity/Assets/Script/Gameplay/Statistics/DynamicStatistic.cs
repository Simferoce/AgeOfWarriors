using System;

namespace Game.Statistics
{
    [Serializable]
    public abstract class DynamicStatistic<T> : Statistic<T>
    {
        protected T current;

        public void Set(T value)
        {
            current = value;
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
    public class DynamicStatisticFloat : DynamicStatistic<float>
    {
        public override bool TryGetDescription(out string description)
        {
            description = string.Empty;
            return false;
        }

        public override string GetFormattedValue(string format)
        {
            return current.ToString(format);
        }

        public override Statistic Snapshot()
        {
            return new DynamicStatisticFloat() { definition = this.definition, current = GetModifiedValue() };
        }
    }
}
