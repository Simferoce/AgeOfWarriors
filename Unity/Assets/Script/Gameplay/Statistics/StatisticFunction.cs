using UnityEngine;

namespace Game.Statistics
{
    public class StatisticFunction<T> : Statistic
    {
        public delegate T StatisticFunctionDelegate();

        private StatisticFunctionDelegate function;

        public StatisticFunction(StatisticFunctionDelegate function, string name, StatisticDefinition definition)
        {
            this.name = name;
            this.function = function;
            this.definition = definition;
        }

        public override U GetValue<U>(object context)
        {
            return StatisticConverter.ConvertGeneric<U, T>(function.Invoke());
        }

        public override string GetDescription(object context)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(Color.white)}>({function.Invoke()})</color>";
        }
    }
}
