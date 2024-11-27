using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BleedingProjectilePerk", menuName = "Definition/Technology/Archer/BleedingProjectilePerk")]
    public class BleedingProjectilePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, BleedingProjectilePerk>, IProjectileModifier
        {
            public Modifier(ModifierHandler modifiable, BleedingProjectilePerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {

            }

            public Game.Modifier GetModifier(Projectile projectile)
            {
                return new BleedingProjectileModifierDefinition.Modifier(
                    projectile.GetCachedComponent<ModifierHandler>(),
                    definition.bleedingProjectileModifierDefinition,
                    definition.percentageAttackPower * modifiable.Entity["attack_power"],
                    definition.duration,
                    Source);
            }
        }

        [SerializeField] private float duration;
        [SerializeField, Range(0, 5)] private float percentageAttackPower;
        [SerializeField] private BleedingProjectileModifierDefinition bleedingProjectileModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(this.Description, StatisticFormatter.Percentage(percentageAttackPower, StatisticDefinition.AttackPower), duration, bleedingProjectileModifierDefinition.BleedingModifierDefinition.MaxStack);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
