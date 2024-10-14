using Game;
using System;

[Serializable]
public abstract class Statistic
{
    public abstract StatisticDefinition GetDefinition(object context);

    public abstract T GetValue<T>(object context);
}