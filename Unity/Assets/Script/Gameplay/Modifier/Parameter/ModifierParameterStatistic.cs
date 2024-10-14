namespace Game
{
    public abstract class ModifierParameterStatistic : ModifierParameter
    {
        public StatisticDefinition StatisticDefinition { get; set; }

        protected ModifierParameterStatistic(string name, StatisticDefinition statisticDefinition)
        {
            StatisticDefinition = statisticDefinition;
            this.Name = name;
        }
    }

    public class ModifierParameterStatistic<T> : ModifierParameterStatistic
    {
        public T Value { get; set; }

        public ModifierParameterStatistic(string name, StatisticDefinition statisticDefinition, T value) : base(name, statisticDefinition)
        {
            Value = value;
        }
    }
}
