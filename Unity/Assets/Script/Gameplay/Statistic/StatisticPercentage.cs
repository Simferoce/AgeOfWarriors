using Game;
using System;
using UnityEngine;

[Serializable]
public class StatisticPercentage : StatisticSerializedIdentity
{
    [SerializeReference, SubclassSelector] private Statistic provider;
    [SerializeField] private float percentage;

    public StatisticPercentage()
    {

    }

    public StatisticPercentage(string name, string definitionId, Statistic provider, float percentage)
        : base(name, definitionId)
    {
        this.provider = provider;
        this.percentage = percentage;
    }

    public override bool TryGetValue<T>(out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, float>(provider * percentage);
        return true;
    }
}