using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ResistKillingBlowPerk>
        {
            private bool hasResists = false;

            public Modifier(ResistKillingBlowPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                {
                    Source.Apply(modifiable, new ResistDeathModifierDefinition.ResistDeath(definition.resistDeathModifierDefinition)
                        .With(new CharacterModifierTimeElement(definition.duration)));
                }
            }
        }

        [SerializeField] private float duration;
        [SerializeField] private ResistDeathModifierDefinition resistDeathModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, duration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
