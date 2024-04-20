using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageDealtReductionModifierDefinition", menuName = "Definition/Modifier/DamageDealtReductionModifierDefinition")]
    public class DamageDealtReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DamageDealtReductionModifierDefinition>
        {
            public Modifier(IModifiable modifiable, DamageDealtReductionModifierDefinition modifierDefinition, float amount) : base(modifiable, modifierDefinition)
            {
            }
        }
    }
}
