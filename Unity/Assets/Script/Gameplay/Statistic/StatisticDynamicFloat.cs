using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatisticDynamicFloat : StatisticDynamic<float>, IStatisticContext
{
    [SerializeReference, SubclassSelector] private Statistic max;

    public Statistic Max { get => max; set => max = value; }

    public override void Initialize(IStatisticContext context)
    {
        base.Initialize(context);

        max.Initialize(context);
    }

    public StatisticDynamicFloat()
    {
    }

    public StatisticDynamicFloat(string name, string definitionId, Statistic max = null)
        : base(name, definitionId)
    {
        this.max = max;
        max.Name = name;
    }

    public override void Modify(float value)
    {
        base.Modify(value);

        if (max != null)
            currentValue = Mathf.Clamp(currentValue, float.MinValue, max);
    }

    public IEnumerable<Statistic> GetStatistic()
    {
        yield return max;
    }
}
