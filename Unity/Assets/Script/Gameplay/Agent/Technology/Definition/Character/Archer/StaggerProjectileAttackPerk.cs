using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerProjectileAttackPerk", menuName = "Definition/Technology/Archer/StaggerProjectileAttackPerk")]
    public class StaggerProjectileAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerProjectileAttackPerk>, IProjectileModifier
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;

            public bool HasModifier => currentAttackApplied >= definition.stack;

            public Modifier(ModifierHandler modifiable, StaggerProjectileAttackPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                affectedAbility = modifiable.Entity.GetCachedComponent<Caster>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
            }

            private void AffectedAbility_OnAbilityEffectApplied()
            {
                if (currentAttackApplied < definition.stack)
                    currentAttackApplied++;
            }

            public override float? GetStack()
            {
                return currentAttackApplied;
            }

            public override void Dispose()
            {
                base.Dispose();
                affectedAbility.OnAbilityEffectApplied -= AffectedAbility_OnAbilityEffectApplied;
            }

            public Game.Modifier GetModifier(Projectile projectile)
            {
                currentAttackApplied = 0;
                return new StaggerProjectileModifierDefinition.Modifier(
                    projectile.Entity.GetCachedComponent<ModifierHandler>(),
                    definition.staggerProjectileAttackPerk,
                    definition.duration,
                    Source);
            }
        }

        [SerializeField] private int stack;
        [SerializeField] private float duration;
        [SerializeField] private AbilityDefinition affectedAbility;
        [SerializeField] private StaggerProjectileModifierDefinition staggerProjectileAttackPerk;

        public override string ParseDescription()
        {
            return string.Format(Description, stack, duration);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
