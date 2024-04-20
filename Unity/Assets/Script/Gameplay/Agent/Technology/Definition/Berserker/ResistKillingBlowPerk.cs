using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ResistKillingBlowPerk>
        {
            private bool hasResists = false;

            public Modifier(IModifiable modifiable, ResistKillingBlowPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                    modifiable.AddModifier(
                        new ResistDeathModifierDefinition.ResistDeath(modifiable, definition.resistDeathModifierDefinition)
                        .With(new CharacterModifierTimeElement(definition.duration)));
            }
        }

        [SerializeField] private ResistDeathModifierDefinition resistDeathModifierDefinition;
        [SerializeField] private float duration;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
