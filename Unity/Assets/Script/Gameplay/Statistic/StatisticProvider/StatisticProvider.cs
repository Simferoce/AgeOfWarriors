using System;

[Serializable]
public abstract class StatisticProvider
{
    public abstract IStatisticContext Resolve(IStatisticContext context);
}