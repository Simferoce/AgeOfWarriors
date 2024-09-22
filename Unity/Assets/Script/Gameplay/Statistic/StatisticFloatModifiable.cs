using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatisticFloatModifiable : StatisticModifiable<float>, IStatisticContext
{
    [SerializeReference, SubclassSelector] private Statistic max;

    public Statistic Max { get => max; set => max = value; }

    public StatisticFloatModifiable()
    {
    }

    public StatisticFloatModifiable(string name, string definitionId, Statistic max)
        : base(name, definitionId)
    {
        this.max = max;
        max.Name = name;
    }

    public override void Modify(float value)
    {
        base.Modify(value);
        currentValue = Mathf.Clamp(currentValue, float.MinValue, max);
    }

    public IEnumerable<Statistic> GetStatistic()
    {
        yield return max;
    }

    public bool IsName(ReadOnlySpan<char> name)
    {
        return name.SequenceEqual(Name);
    }
}
