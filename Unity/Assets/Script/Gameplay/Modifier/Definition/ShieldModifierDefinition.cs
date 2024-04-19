using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldModifierDefinition", menuName = "Definition/Modifier/ShieldModifierDefinition")]
    public class ShieldModifierDefinition : ModifierDefinition
    {
        public class Shield : Modifier<Shield>
        {
            public float Remaining { get; set; }
            public float Initial { get; set; }

            public override bool TryGetValue<T>(StatisticDefinition definition, out T value)
            {
                if (definition == StatisticDefinition.Shield)
                {
                    value = (T)(object)Remaining;
                    return true;
                }

                return base.TryGetValue(definition, out value);
            }

            public Shield(IModifiable modifiable, ModifierDefinition modifierDefinition, float initial) : base(modifiable, modifierDefinition)
            {
                Initial = initial;

                Remaining = Initial;
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
            return new Shield(modifiable, this, initial).With(new CharacterModifierTimeElement(duration));
        }
    }
}
