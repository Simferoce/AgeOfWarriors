using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class FloatStatisticModifierParameterFactory : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ModifierParameter Create(object entity)
        {
            return new StatisticModifierParameter<float>(name, statistic.Definition, statistic.GetValue<float>(entity));
        }

        public override void Initialize(object context)
        {
            if (context is Entity entity)
                entity.AddOrGetCachedComponent<StatisticIndex>().Add(statistic);
        }
    }
}
