using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseModifierDefinition", menuName = "Definition/Modifier/DefenseModifierDefinition")]
    public class DefenseModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseModifierDefinition>
        {
            public object Source { get; set; }
            public override float? Defense => defense;

            private float defense;

            public Modifier(Character character, DefenseModifierDefinition modifierDefinition, float defense, object source)
                : base(character.GetCachedComponent<IModifiable>(), modifierDefinition)
            {
                this.defense = defense;
                this.Source = source;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, defense);
            }
        }
    }
}
