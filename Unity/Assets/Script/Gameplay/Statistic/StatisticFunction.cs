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

    public override bool TryGetValue<T>(out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(function(base.Context as Context));
        return true;
    }
}

