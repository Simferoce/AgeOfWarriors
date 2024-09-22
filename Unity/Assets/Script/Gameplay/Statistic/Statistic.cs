using Game;
using System;

[Serializable]
public abstract class Statistic
{
    public abstract string Name { get; set; }
    public abstract StatisticDefinition Definition { get; set; }
    protected IStatisticContext Context { get => context != null ? context : throw new Exception($"Uninitialized context"); set => context = value; }

    protected IStatisticContext context;

    public virtual void Initialize(IStatisticContext context)
    {
        this.context = context;
    }

    public T GetValueOrThrow<T>()
    {
        return TryGetValue<T>(out T value) ? value : throw new Exception($"Unable to get the statistic from {Name} with context {Context}");
    }

    public T GetValueOrDefault<T>(T defaultValue = default)
    {
        return TryGetValue<T>(out T value) ? value : defaultValue;
    }

    public abstract bool TryGetValue<T>(out T value);

    public static implicit operator bool(Statistic d) => d.GetValueOrThrow<bool>();
    public static implicit operator int(Statistic d) => d.GetValueOrThrow<int>();
    public static implicit operator float(Statistic d) => d.GetValueOrThrow<float>();
}