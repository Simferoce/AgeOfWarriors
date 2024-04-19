using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageDealtReductionModifierDefinition", menuName = "Definition/Modifier/DamageDealtReductionModifierDefinition")]
    public class DamageDealtReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }
    }
}
