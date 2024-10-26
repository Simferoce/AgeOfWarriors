using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierParameterFactory<T> : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticReference<T> statistic;

        public override ModifierParameter Create(object entity)
        {
            return new StatisticModifierParameter<T>(name, statistic.Resolve(entity as Entity));
        }
    }

    [Serializable]
    public class StatisticModifierParameterFactory : StatisticModifierParameterFactory<float>
    {

    }
}
