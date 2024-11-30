using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistDeathModifierDefinition", menuName = "Definition/Modifier/ResistDeathModifierDefinition")]
    public class ResistDeathModifierDefinition : ModifierDefinition
    {
        public override Modifier Instantiate()
        {
            throw new System.NotImplementedException();
        }

        public class ResistDeath : Modifier<ResistDeath, ResistDeathModifierDefinition>
        {
            public override bool? IsInvulnerable => true;

            public ResistDeath(ResistDeathModifierDefinition modifierDefinition) : base(modifierDefinition)
            {
            }

            public override string ParseDescription()
            {
                return definition.Description;
            }
        }
    }
}
