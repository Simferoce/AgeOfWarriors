using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackPowerModifierDefinition", menuName = "Definition/Modifier/AttackPowerModifierDefinition")]
    public class AttackPowerModifierDefinition : ModifierDefinition
    {
        public class AttackPowerModifier : Modifier<AttackPowerModifier, AttackPowerModifierDefinition>
        {
            private Statistic<float> attackPowerFlat;

            public AttackPowerModifier(ModifierHandler modifiable, AttackPowerModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.AttackPowerFlat, amount);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, attackPowerFlat.GetValue());
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }
    }
}
