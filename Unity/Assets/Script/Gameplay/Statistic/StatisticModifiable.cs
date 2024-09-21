using Game;
using System;

[Serializable]
public class StatisticModifiable<Type> : StatisticSerializedIdentity
{
    protected Type currentValue;

    public StatisticModifiable()
    {

    }

    public StatisticModifiable(string name, string definitionId)
        : base(name, definitionId)
    {

    }

    public virtual void Modify(IStatisticContext context, Type value)
    {
        currentValue = value;
    }

    public override bool TryGetValue<T>(IStatisticContext context, out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(currentValue);
        return true;
    }
}

