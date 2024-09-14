using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageAgainstWeakPerk", menuName = "Definition/Technology/Shieldbearer/DamageAgainstWeakPerk")]
    public class DamageAgainstWeakPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DamageAgainstWeakPerk>
        {
            public override float? DamageDealtAgainstWeak => definition.damageDealtAgainstWeak;

            public Modifier(ModifierHandler modifiable, DamageAgainstWeakPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
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
