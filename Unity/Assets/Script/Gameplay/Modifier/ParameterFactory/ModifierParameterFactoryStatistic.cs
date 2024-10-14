using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierParameterFactoryStatisticFloat : ModifierParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ModifierParameter Create(Modifier modifier)
        {
            return new ModifierParameterStatistic<float>(name, statistic.GetDefinition(modifier), statistic.GetValue<float>(modifier));
        }
    }
}
