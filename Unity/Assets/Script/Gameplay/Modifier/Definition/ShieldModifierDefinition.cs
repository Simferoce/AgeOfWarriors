using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldModifierDefinition", menuName = "Definition/Modifier/ShieldModifierDefinition")]
    public class ShieldModifierDefinition : ModifierDefinition
    {
        public class Shield : Modifier<Shield, ShieldModifierDefinition>
        {
            public float Remaining { get; set; }
            public float Initial { get; set; }

            public Shield(IModifiable modifiable, ShieldModifierDefinition modifierDefinition, float initial, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                Initial = initial;

                Remaining = Initial;
            }

            public override string ParseDescription()
            {
                return string.Format(Definition.Description, Remaining);
            }

            public bool Absorb(float amount, out float amountNotAbsorbed)
            {
                float absortion = Remaining - amount;
                Remaining -= Mathf.Min(Remaining, amount);
                amountNotAbsorbed = absortion < 0 ? -absortion : 0;

                return Remaining > 0;
            }
        }

        public Shield CreateShield(IModifiable modifiable, float initial, float duration)
        {
            return new Shield(
                    modifiable,
                    this,
                    initial,
                    modifiable.GetCachedComponent<IModifierSource>())
                .With(new CharacterModifierTimeElement(duration));
        }
    }
}
