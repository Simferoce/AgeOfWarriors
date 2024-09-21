using Game;
using System;

[Serializable]
public class StatisticFunction<Context, Type> : StatisticSerializedIdentity
    where Context : class
{
    private Func<Context, Type> function;

    public StatisticFunction(string name, string definitionId, Func<Context, Type> function)
        : base(name, definitionId)
    {
        this.function = function;
    }

    public override bool TryGetValue<T>(IStatisticContext context, out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(function(context as Context));
        return true;
    }
}

