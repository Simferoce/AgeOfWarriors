using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedModifierDefinition", menuName = "Definition/Modifier/AttackSpeedModifierDefinition")]
    public class AttackSpeedReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedReductionModifierDefinition>
        {
            private StatisticModifiable<float> attackSpeedPercentage = new StatisticModifiable<float>(definition: StatisticRepository.AttackSpeedPercentage);

            public Modifier(ModifierHandler modifiable, AttackSpeedReductionModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackSpeedPercentage.Initialize(this);
                attackSpeedPercentage.Modify(amount);
            }
        }
    }
}
