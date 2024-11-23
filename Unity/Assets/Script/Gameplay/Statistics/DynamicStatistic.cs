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

        public override T GetBaseValue(Context context)
        {
            return current;
        }

        public override T GetModifiedValue(Context context)
        {
            return definition.Modify(current, entity.GetCachedComponent<StatisticRepository>(), context);
        }
    }

    [Serializable]
    public class DynamicStatisticFloat : DynamicStatistic<float>
    {
        public override bool TryGetDescription(out string description, Context context)
        {
            description = string.Empty;
            return false;
        }

        public override string GetFormattedValue(string format, Context context)
        {
            return current.ToString(format);
        }

        public override Statistic Snapshot(Context context)
        {
            return new DynamicStatisticFloat() { definition = this.definition, current = GetModifiedValue(context) };
        }
    }
}
