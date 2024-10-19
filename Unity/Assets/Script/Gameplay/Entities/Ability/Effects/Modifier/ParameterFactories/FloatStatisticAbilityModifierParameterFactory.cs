using Game.Modifier;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class FloatStatisticAbilityModifierParameterFactory : AbilityModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ModifierParameter Create(AbilityEntity ability)
        {
            return new StatisticModifierParameter<float>(name, statistic.Definition, statistic.GetValue<float>(ability));
        }
    }
}
