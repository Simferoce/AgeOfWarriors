using System;
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
        max.SetName(null, "max");
    }

    public override void Modify(IStatisticContext context, float value)
    {
        base.Modify(context, value);
        currentValue = Mathf.Clamp(currentValue, float.MinValue, max.GetValueOrThrow<float>(context));
    }

    public IStatisticContext GetContext(ReadOnlySpan<char> value)
    {
        return null;
    }

    public Statistic GetStatistic(ReadOnlySpan<char> value)
    {
        if (value.SequenceEqual("max"))
            return max;

        return null;
    }

    public bool IsName(ReadOnlySpan<char> name)
    {
        return name.SequenceEqual(this.GetName(this));
    }
}
