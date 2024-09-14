using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldModifierDefinition", menuName = "Definition/Modifier/ShieldModifierDefinition")]
    public class ShieldModifierDefinition : ModifierDefinition
    {
        public class Shield : Modifier<Shield, ShieldModifierDefinition>
        {
            public event System.Action<Shield> OnDestroyed;

            public float Remaining { get; set; }
            public float Initial { get; set; }

            public Shield(ModifierHandler modifiable, ShieldModifierDefinition modifierDefinition, float initial, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
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

                if (Remaining <= 0)
                    Modifiable.RemoveModifier(this);

                return Remaining > 0;
            }

            public override void Dispose()
            {
                base.Dispose();
                OnDestroyed?.Invoke(this);
            }
        }

        public Shield CreateShield(ModifierHandler modifiable, float initial, float duration)
        {
            return new Shield(
                    modifiable,
                    this,
                    initial,
                    modifiable.Entity.GetCachedComponent<IModifierSource>())
                .With(new CharacterModifierTimeElement(duration));
        }
    }
}
