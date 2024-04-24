using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BleedingProjectilePerk", menuName = "Definition/Technology/Archer/BleedingProjectilePerk")]
    public class BleedingProjectilePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, BleedingProjectilePerk>, IProjectileModifier
        {
            public Modifier(IModifiable modifiable, BleedingProjectilePerk modifierDefinition) : base(modifiable, modifierDefinition)
            {

            }

            public Game.Modifier GetModifier(Projectile projectile)
            {
                return new BleedingProjectileModifierDefinition.Modifier(projectile.GetCachedComponent<IModifiable>(), definition.bleedingProjectileModifierDefinition, definition.percentageAttackPower * modifiable.GetCachedComponent<Character>().AttackPower, definition.duration);
            }
        }

        [SerializeField] private float duration;
        [SerializeField, Range(0, 5)] private float percentageAttackPower;
        [SerializeField] private BleedingProjectileModifierDefinition bleedingProjectileModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
