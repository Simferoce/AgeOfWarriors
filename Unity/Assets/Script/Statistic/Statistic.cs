using System;

namespace Game
{
    [Serializable]
    public abstract class Statistic
    {
        protected object owner;

        public void Initialize(object owner)
        {
            this.owner = owner;
        }
    }

    [Serializable]
    public abstract class Statistic<T> : Statistic
    {
        public abstract StatisticDefinition GetDefinition(StatisticContext context);
        public abstract T GetValue(StatisticContext context);

        public T GetValue()
        {
            return GetValue(new StatisticContext());
        }
    }

    public interface IStatisticFloat
    {
        public abstract float GetValue();
        public abstract float GetValue(StatisticContext context);
    }
}
