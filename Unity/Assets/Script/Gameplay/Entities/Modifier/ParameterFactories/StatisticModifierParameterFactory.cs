using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierParameterFactory<T> : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticReference<T> reference;

        public override ModifierParameter Create(object entity)
        {
            return new ModifierParameter<T>(name, reference.Resolve(entity as Entity).GetValue<T>(null));
        }
    }

    [Serializable]
    public class StatisticModifierParameterFactory : StatisticModifierParameterFactory<float>
    {

    }
}
