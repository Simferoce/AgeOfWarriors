using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerProjectileAttackPerk", menuName = "Definition/Technology/Archer/StaggerProjectileAttackPerk")]
    public class StaggerProjectileAttackPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerProjectileAttackPerk>, IProjectileModifier
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;

            public bool HasModifier => currentAttackApplied >= definition.stack;

            public Modifier(StaggerProjectileAttackPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
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
                    projectile.GetCachedComponent<ModifierHandler>(),
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

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
