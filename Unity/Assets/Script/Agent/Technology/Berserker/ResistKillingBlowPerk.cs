using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private class ResistDeath : Modifier<ResistDeath>
            {
                public override bool? Invulnerable => true;

                public ResistDeath(IModifiable modifiable) : base(modifiable)
                {
                }
            }

            private bool hasResists = false;
            private float invulnerabilityPeriod;

            public Modifier(IModifiable modifiable, float invulnerabilityPeriod) : base(modifiable)
            {
                this.invulnerabilityPeriod = invulnerabilityPeriod;
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                    modifiable.AddModifier(new ResistDeath(modifiable).With(new CharacterModifierTimeElement(invulnerabilityPeriod)));
            }
        }

        [SerializeField] private float invulnerabilityPeriod;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, invulnerabilityPeriod);
        }
    }
}
