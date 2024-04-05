using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "InspiringPresenceModifierDefinition", menuName = "Definition/Modifier/InspiringPresenceModifierDefinition")]
    public class InspiringPresenceModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public InspiringPresenceAbilityEffect Source { get; set; }
            public override float? Defense => defense;

            private float defense;

            public Modifier(Character character, ModifierDefinition modifierDefinition, float defense, InspiringPresenceAbilityEffect source)
                : base(character, modifierDefinition)
            {
                this.defense = defense;
                this.Source = source;
            }
        }
    }
}
