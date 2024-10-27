using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierParameterFactory<T> : ModifierParameterFactory
    {
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            statistic.Initialize(entity);
        }

        public override ModifierParameter Create(object entity)
        {
            return new ModifierParameter<T>(statistic.Name, statistic.GetValue<T>(null));
        }
    }

    [Serializable]
    public class StatisticModifierParameterFactory : StatisticModifierParameterFactory<float>
    {

    }
}
