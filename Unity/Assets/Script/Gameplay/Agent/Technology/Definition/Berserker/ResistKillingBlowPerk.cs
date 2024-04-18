using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private bool hasResists = false;
            private ResistDeathModifierDefinition resistDeathModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, ResistDeathModifierDefinition resistDeathModifierDefinition) : base(modifiable, modifierDefinition)
            {
                this.resistDeathModifierDefinition = resistDeathModifierDefinition;
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                    modifiable.AddModifier(new ResistDeathModifierDefinition.ResistDeath(modifiable, resistDeathModifierDefinition).With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration))));
            }
        }

        [SerializeField] private ResistDeathModifierDefinition resistDeathModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, resistDeathModifierDefinition);
        }
    }
}
