using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class GameModifierParameterFactoryStatisticFloat : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ModifierParameter Create(ModifierEntity modifier)
        {
            return new ModifierParameterStatistic<float>(name, statistic.Definition, statistic.GetValue<float>(modifier));
        }
    }
}
