using Game;
using System;
using UnityEngine;

[Serializable]
public class StatisticSerialize<Type> : StatisticSerializedIdentity
{
    [SerializeField]
    private Type value;

    public StatisticSerialize(string name, string definitionId, Type value)
        : base(name, definitionId)
    {
        this.value = value;
    }

    public override bool TryGetValue<T>(IStatisticContext context, out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(this.value);
        return true;
    }
}

