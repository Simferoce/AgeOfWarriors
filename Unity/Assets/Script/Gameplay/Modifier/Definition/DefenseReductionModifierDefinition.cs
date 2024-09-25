using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseReductionModifierDefinition", menuName = "Definition/Modifier/DefenseReductionModifierDefinition")]
    public class DefenseReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseReductionModifierDefinition>
        {
            private StatisticModifiable<float> defenseReduction = new StatisticModifiable<float>(definition: StatisticRepository.DefenseReduction);

            public Modifier(ModifierHandler modifiable, DefenseReductionModifierDefinition modifierDefinition, float duration, float reduction, IModifierSource source = null)
                : base(modifiable, modifierDefinition, source)
            {
                With(new CharacterModifierTimeElement(duration));
                defenseReduction.Initialize(this);
                defenseReduction.Modify(reduction);
            }

            public override IEnumerable<Statistic> GetStatistic()
            {
                yield return defenseReduction;

                foreach (Statistic statistic in base.GetStatistic())
                    yield return statistic;
            }
        }
    }
}
