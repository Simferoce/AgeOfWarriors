using Game;
using System;

[Serializable]
public abstract class Statistic
{
    public abstract StatisticDefinition GetDefinition(IStatisticContext context);
    public abstract string GetName(IStatisticContext context);
    public abstract string SetName(IStatisticContext context, string value);

    public T GetValueOrThrow<T>(IStatisticContext context)
    {
        return TryGetValue<T>(context, out T value) ? value : throw new Exception($"Unable to get the statistic from {GetName(context)} with context {context}");
    }

    public T GetValueOrDefault<T>(IStatisticContext context, T defaultValue = default)
    {
        return TryGetValue<T>(context, out T value) ? value : defaultValue;
    }

    public abstract bool TryGetValue<T>(IStatisticContext context, out T value);
}