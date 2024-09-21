using Game;
using System;
using UnityEngine;

[Serializable]
public abstract class Statistic
{
    [SerializeField]
    protected string name;

    [SerializeField]
    protected string definitionId;

    public StatisticDefinition Definition { get => StatisticRepository.GetDefinition(definitionId); }
    public string Name { get => name; set => name = value; }

    protected Statistic()
    {
    }

    protected Statistic(string name, string definitionId)
    {
        this.name = name;
        this.definitionId = definitionId;
    }

    public T GetValueOrThrow<T>(IStatisticContext context)
    {
        return TryGetValue<T>(context, out T value) ? value : throw new Exception($"Unable to get the statistic from {name} with context {context}");
    }

    public T GetValueOrDefault<T>(IStatisticContext context, T defaultValue = default)
    {
        return TryGetValue<T>(context, out T value) ? value : defaultValue;
    }

    public abstract bool TryGetValue<T>(IStatisticContext context, out T value);
}