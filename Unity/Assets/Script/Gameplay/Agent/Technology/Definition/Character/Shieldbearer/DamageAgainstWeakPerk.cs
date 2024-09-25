using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageAgainstWeakPerk", menuName = "Definition/Technology/Shieldbearer/DamageAgainstWeakPerk")]
    public class DamageAgainstWeakPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DamageAgainstWeakPerk>
        {
            private StatisticModifiable<float> damageAgainstWeak = new StatisticModifiable<float>(definition: StatisticRepository.Damage);

            public Modifier(ModifierHandler modifiable, DamageAgainstWeakPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                damageAgainstWeak.Initialize(this);
                damageAgainstWeak.Modify(definition.damageDealtAgainstWeak);
            }
        }

        [SerializeField] private float damageDealtAgainstWeak;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
