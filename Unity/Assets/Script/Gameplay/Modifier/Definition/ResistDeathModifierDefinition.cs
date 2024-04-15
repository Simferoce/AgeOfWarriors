using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistDeathModifierDefinition", menuName = "Definition/Modifier/ResistDeathModifierDefinition")]
    public class ResistDeathModifierDefinition : ModifierDefinition
    {
        public class ResistDeath : Modifier<ResistDeath>
        {
            public override bool? Invulnerable => true;

            public ResistDeath(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }
    }
}
