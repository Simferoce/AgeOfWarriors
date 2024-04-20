using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "InspiringPresenceModifierDefinition", menuName = "Definition/Modifier/InspiringPresenceModifierDefinition")]
    public class InspiringPresenceModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, InspiringPresenceModifierDefinition>
        {
            public InspiringPresenceAbilityEffect Source { get; set; }
            public override float? Defense => defense;

            private float defense;

            public Modifier(Character character, InspiringPresenceModifierDefinition modifierDefinition, float defense, InspiringPresenceAbilityEffect source)
                : base(character.GetCachedComponent<IModifiable>(), modifierDefinition)
            {
                this.defense = defense;
                this.Source = source;
            }
        }
    }
}
