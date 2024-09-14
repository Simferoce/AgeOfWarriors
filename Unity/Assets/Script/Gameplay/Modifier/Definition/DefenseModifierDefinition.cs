using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseModifierDefinition", menuName = "Definition/Modifier/DefenseModifierDefinition")]
    public class DefenseModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseModifierDefinition>
        {
            public override float? Defense => defense;

            private float defense;

            public Modifier(Character character, DefenseModifierDefinition modifierDefinition, float defense, IModifierSource source)
                : base(character.Entity.GetCachedComponent<ModifierHandler>(), modifierDefinition, source)
            {
                this.defense = defense;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, defense);
            }
        }
    }
}
