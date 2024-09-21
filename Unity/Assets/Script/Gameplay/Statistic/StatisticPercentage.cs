using Game;
using System;
using UnityEngine;

[Serializable]
public class StatisticPercentage : Statistic
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

    public override bool TryGetValue<T>(IStatisticContext context, out T value)
    {
        value = StatisticUtility.ConvertGeneric<T, float>(provider.GetValueOrThrow<float>(context) * percentage);
        return true;
    }
}