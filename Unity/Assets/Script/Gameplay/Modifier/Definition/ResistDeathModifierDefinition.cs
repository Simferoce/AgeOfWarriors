using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistDeathModifierDefinition", menuName = "Definition/Modifier/ResistDeathModifierDefinition")]
    public class ResistDeathModifierDefinition : ModifierDefinition
    {
        public class ResistDeath : Modifier<ResistDeath, ResistDeathModifierDefinition>
        {
            public override bool? Invulnerable => true;

            public ResistDeath(IModifiable modifiable, ResistDeathModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }

            public override string ParseDescription()
            {
                return definition.Description;
            }
        }
    }
}
