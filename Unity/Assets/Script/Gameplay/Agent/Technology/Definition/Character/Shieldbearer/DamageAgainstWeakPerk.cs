using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageAgainstWeakPerk", menuName = "Definition/Technology/Shieldbearer/DamageAgainstWeakPerk")]
    public class DamageAgainstWeakPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DamageAgainstWeakPerk>
        {
            private Statistic<float> damageDealtAgainstWeak;

            public Modifier(ModifierHandler modifiable, DamageAgainstWeakPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                damageDealtAgainstWeak = new Statistic<float>(StatisticDefinition.DamageDealtAgainstWeak, definition.damageDealtAgainstWeak);
                StatisticRegistry.Register(damageDealtAgainstWeak);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(damageDealtAgainstWeak);
            }
        }

        [SerializeField] private float damageDealtAgainstWeak;

        public override string ParseDescription()
        {
            return string.Format(Description, damageDealtAgainstWeak);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
