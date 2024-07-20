using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageDealtReductionModifierDefinition", menuName = "Definition/Modifier/DamageDealtReductionModifierDefinition")]
    public class DamageDealtReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DamageDealtReductionModifierDefinition>
        {
            public override float? DamageDealtReduction => amount;

            private float amount;

            public Modifier(IModifiable modifiable, DamageDealtReductionModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
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
