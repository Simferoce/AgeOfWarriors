namespace Game
{
    public class StatisticFunction<T> : Statistic
    {
        public delegate T StatisticFunctionDelegate();

        private StatisticFunctionDelegate function;
        private StatisticDefinition definition;

        public StatisticFunction(StatisticFunctionDelegate function, StatisticDefinition definition)
        {
            this.function = function;
            this.definition = definition;
        }

        public override StatisticDefinition GetDefinition(object context)
        {
            return definition;
        }

        public override U GetValue<U>(object context)
        {
            return StatisticUtility.ConvertGeneric<U, T>(function.Invoke());
        }
    }
}
