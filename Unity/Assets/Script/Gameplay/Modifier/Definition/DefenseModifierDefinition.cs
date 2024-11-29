using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseModifierDefinition", menuName = "Definition/Modifier/DefenseModifierDefinition")]
    public class DefenseModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseModifierDefinition>
        {
            private float defense;
            private Statistic<float> defenseFlat;

            public Modifier(Character character, DefenseModifierDefinition modifierDefinition, float defense, IModifierSource source)
                : base(character.GetCachedComponent<ModifierHandler>(), modifierDefinition, source)
            {
                this.defense = defense;
                defenseFlat = new Statistic<float>(StatisticDefinition.FlatDefense, defense);
                StatisticRegistry.Register(defenseFlat);
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, defense);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(defenseFlat);
            }
        }
    }
}
