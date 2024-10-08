﻿using Game;
using System;
using UnityEngine;

[Serializable]
public class StatisticSerializeFloat : StatisticSerialize<float>
{
    public StatisticSerializeFloat()
    {

    }

    public StatisticSerializeFloat(string name, string definitionId, float value)
        : base(name, definitionId, value)
    {
    }
}

[Serializable]
public class StatisticSerialize<Type> : StatisticSerializedIdentity
{
    [SerializeField]
    private Type value;

    public StatisticSerialize()
    {

    }

    public StatisticSerialize(string name, string definitionId, Type value)
        : base(name, definitionId)
    {
        this.value = value;
    }

    public override bool TryGetValue<T>(out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, Type>(this.value);
        return true;
    }
}