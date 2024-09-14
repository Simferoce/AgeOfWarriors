using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistDeathModifierDefinition", menuName = "Definition/Modifier/ResistDeathModifierDefinition")]
    public class ResistDeathModifierDefinition : ModifierDefinition
    {
        public class ResistDeath : Modifier<ResistDeath, ResistDeathModifierDefinition>
        {
            public override bool? IsInvulnerable => true;

            public ResistDeath(ModifierHandler modifiable, ResistDeathModifierDefinition modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }

            public override string ParseDescription()
            {
                return definition.Description;
            }
        }
    }
}
