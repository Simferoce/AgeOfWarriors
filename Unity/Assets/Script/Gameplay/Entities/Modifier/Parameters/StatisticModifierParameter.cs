using Game.Statistics;

namespace Game.Modifier
{
    public class StatisticModifierParameter<T> : ModifierParameter<T>
    {
        public Statistic<T> Statistic { get; set; }

        public StatisticModifierParameter(string name, Statistic<T> statistic) : base(name)
        {
            Statistic = statistic;
        }

        public override T GetValue(Context context)
        {
            return Statistic.GetValue<T>(context);
        }
    }
}
