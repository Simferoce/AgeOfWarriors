using Game.Statistics;

namespace Game.Projectile
{
    public abstract class StatisticProjectileParameter : ProjectileParameter
    {
        public StatisticDefinition StatisticDefinition { get; set; }

        protected StatisticProjectileParameter(string name, StatisticDefinition statisticDefinition)
        {
            StatisticDefinition = statisticDefinition;
            this.Name = name;
        }

        public abstract string GetDescription();
    }

    public class StatisticProjectileParameter<T> : StatisticProjectileParameter
    {
        public T Value { get; set; }

        public StatisticProjectileParameter(string name, StatisticDefinition statisticDefinition, T value) : base(name, statisticDefinition)
        {
            Value = value;
        }

        public override string GetDescription()
        {
            if (StatisticDefinition.IsPercentage)
                return $"<color=#{StatisticDefinition.ColorHex}>({Value:0.0%}{StatisticDefinition.TextIcon})</color>";
            else
                return $"<color=#{StatisticDefinition.ColorHex}>({Value}{StatisticDefinition.TextIcon})</color>";
        }
    }
}
