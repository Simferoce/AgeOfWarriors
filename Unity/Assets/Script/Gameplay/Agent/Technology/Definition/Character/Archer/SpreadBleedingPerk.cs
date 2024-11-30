using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpreadBleedingPerk", menuName = "Definition/Technology/Archer/SpreadBleedingPerk")]
    public class SpreadBleedingPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpreadBleedingPerk>
        {
            public float SpreadDistance => definition.spreadDistance;

            public Modifier(SpreadBleedingPerk modifierDefinition) : base(modifierDefinition)
            {
            }
        }

        [SerializeField] private float spreadDistance;

        public override string ParseDescription()
        {
            return string.Format(Description, spreadDistance);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
