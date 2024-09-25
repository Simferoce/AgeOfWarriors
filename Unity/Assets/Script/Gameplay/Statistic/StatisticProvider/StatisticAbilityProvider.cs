using System;

[Serializable]
public abstract class StatisticAbilityProvider : StatisticProvider
{
    public abstract override IStatisticContext Resolve(IStatisticContext context);
}