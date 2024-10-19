namespace Game.Statistics
{
    public class StatisticFunction<T> : Statistic
    {
        public delegate T StatisticFunctionDelegate();

        private StatisticFunctionDelegate function;

        public StatisticFunction(StatisticFunctionDelegate function, StatisticDefinition definition)
        {
            this.function = function;
            this.definition = definition;
        }

        public override U GetValue<U>(object context)
        {
            return StatisticConverter.ConvertGeneric<U, T>(function.Invoke());
        }
    }
}
