using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageAgainstWeakPerk", menuName = "Definition/Technology/Shieldbearer/DamageAgainstWeakPerk")]
    public class DamageAgainstWeakPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DamageAgainstWeakPerk>
        {
            public override float? DamageDealtAgainstWeak => definition.damageDealtAgainstWeak;

            public Modifier(IModifiable modifiable, DamageAgainstWeakPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }
        }

        [SerializeField] private float damageDealtAgainstWeak;

        public override string ParseDescription()
        {
            return string.Format(Description, damageDealtAgainstWeak);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}
