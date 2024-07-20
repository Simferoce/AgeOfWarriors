using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedModifierDefinition", menuName = "Definition/Modifier/AttackSpeedModifierDefinition")]
    public class AttackSpeedReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedReductionModifierDefinition>
        {
            private float amount;

            public override float? AttackSpeedPercentage => -amount;

            public Modifier(IModifiable modifiable, AttackSpeedReductionModifierDefinition modifierDefinition, float amount) : base(modifiable, modifierDefinition)
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
