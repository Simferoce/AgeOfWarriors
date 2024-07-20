using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackPowerModifierDefinition", menuName = "Definition/Modifier/AttackPowerModifierDefinition")]
    public class AttackPowerModifierDefinition : ModifierDefinition
    {
        public class AttackPowerModifier : Modifier<AttackPowerModifier, AttackPowerModifierDefinition>
        {
            private float amount;

            public override float? AttackPower => amount;

            public AttackPowerModifier(IModifiable modifiable, AttackPowerModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.amount = amount;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, amount);
            }
        }
    }
}
