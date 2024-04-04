using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float invulnerabilityPeriod;
            private float? lastKillingBlowResult = null;

            public override bool? Invulnerable => lastKillingBlowResult != null && Time.time - lastKillingBlowResult < invulnerabilityPeriod;

            public Modifier(IModifiable modifiable, float invulnerabilityPeriod) : base(modifiable)
            {
                this.invulnerabilityPeriod = invulnerabilityPeriod;
            }

            public void OnKillingBlowResisted()
            {
                if (lastKillingBlowResult == null)
                    lastKillingBlowResult = Time.time;
            }
        }

        [SerializeField] private float invulnerabilityPeriod;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, invulnerabilityPeriod);
        }
    }
}
