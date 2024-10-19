using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class FloatStatisticModifierParameterFactory : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private ModifierStatistic statistic;

        public override ModifierParameter Create(ModifierEntity modifier)
        {
            return new StatisticModifierParameter<float>(name, statistic.Definition, statistic.GetValue<float>(modifier));
        }
    }
}
