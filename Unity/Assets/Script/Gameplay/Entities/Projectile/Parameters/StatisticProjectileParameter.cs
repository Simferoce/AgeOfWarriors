using Game.Statistics;

namespace Game.Projectile
{
    public class StatisticProjectileParameter<T> : ProjectileParameter<T>
    {
        public Statistic<T> Statistic { get; set; }

        public StatisticProjectileParameter(string name, Statistic<T> statistic) : base(name)
        {
            Statistic = statistic;
        }

        public override T GetValue(Context context)
        {
            return Statistic.GetValue(context);
        }
    }
}
