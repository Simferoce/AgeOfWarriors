using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ConfusionModifierDefinition", menuName = "Definition/Modifier/ConfusionModifierDefinition")]
    public class ConfusionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ConfusionModifierDefinition>
        {
            public override bool? IsConfused => true;

            public Modifier(IModifiable modifiable, ConfusionModifierDefinition modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }
    }
}
