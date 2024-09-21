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
                    projectile.Entity.GetCachedComponent<ModifierHandler>(),
                    definition.bleedingProjectileModifierDefinition,
                    definition.Damage.GetValueOrThrow<float>(this),
                    definition.Duration.GetValueOrThrow<float>(this),
                    Source);
            }
        }

        [SerializeField] public StatisticSerialize<float> Duration = new StatisticSerialize<float>("duration", StatisticRepository.Duration, 1f);
        [SerializeField] public StatisticPercentage Damage = new StatisticPercentage("damage", StatisticRepository.Damage, new StatisticReference("source.attack_power"), 1f);
        [SerializeField] private BleedingProjectileModifierDefinition bleedingProjectileModifierDefinition;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
