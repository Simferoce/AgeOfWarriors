using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierParameterFactory<T> : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticReference<T> value;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            value.Initialize(entity);
        }

        public override ModifierParameter Create(object entity)
        {
            return new ModifierParameter<T>(name, value.Get().GetModifiedValue<T>(Context.Empty));
        }
    }

    [Serializable]
    public class StatisticModifierParameterFactory : StatisticModifierParameterFactory<float>
    {

    }

    [Serializable]
    public class StatisticIntegerModifierParameterFactory : StatisticModifierParameterFactory<int>
    {

    }
}
