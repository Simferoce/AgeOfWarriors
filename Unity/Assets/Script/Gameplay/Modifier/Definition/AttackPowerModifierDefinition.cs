using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackPowerModifierDefinition", menuName = "Definition/Modifier/AttackPowerModifierDefinition")]
    public class AttackPowerModifierDefinition : ModifierDefinition
    {
        public class AttackPowerModifier : Modifier<AttackPowerModifier, AttackPowerModifierDefinition>
        {
            private StatisticModifiable<float> attackPower = new StatisticModifiable<float>(definition: StatisticRepository.AttackPower);

            public AttackPowerModifier(ModifierHandler modifiable, AttackPowerModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackPower.Initialize(this);
                attackPower.Modify(amount);
            }
        }
    }
}
