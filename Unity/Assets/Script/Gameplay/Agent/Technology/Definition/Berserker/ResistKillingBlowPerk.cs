using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private bool hasResists = false;
            private float invulnerabilityPeriod;
            private ResistDeathModifierDefinition resistDeathModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float invulnerabilityPeriod, ResistDeathModifierDefinition resistDeathModifierDefinition) : base(modifiable, modifierDefinition)
            {
                this.invulnerabilityPeriod = invulnerabilityPeriod;
                this.resistDeathModifierDefinition = resistDeathModifierDefinition;
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                    modifiable.AddModifier(new ResistDeathModifierDefinition.ResistDeath(modifiable, resistDeathModifierDefinition).With(new CharacterModifierTimeElement(invulnerabilityPeriod)));
            }
        }

        [SerializeField] private float invulnerabilityPeriod;
        [SerializeField] private ResistDeathModifierDefinition resistDeathModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, invulnerabilityPeriod, resistDeathModifierDefinition);
        }
    }
}
