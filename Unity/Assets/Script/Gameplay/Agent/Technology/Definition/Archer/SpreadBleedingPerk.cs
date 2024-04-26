using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpreadBleedingPerk", menuName = "Definition/Technology/Archer/SpreadBleedingPerk")]
    public class SpreadBleedingPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, SpreadBleedingPerk>
        {
            public float SpreadDistance => definition.spreadDistance;

            public Modifier(IModifiable modifiable, SpreadBleedingPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        [SerializeField] private float spreadDistance;

        public override string ParseDescription()
        {
            return string.Format(Description, spreadDistance);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
