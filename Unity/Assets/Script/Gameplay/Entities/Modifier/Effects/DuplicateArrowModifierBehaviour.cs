using Game.Ability;
using Game.Agent;
using Game.Components;
using Game.Projectile;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class DuplicateArrowModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        [SerializeField] private AbilityDefinition abilityAffected;
        [SerializeField] private StatisticReference threshold;
        [SerializeField] private StatisticReference range;

        private ProjectileAbilityEffect abilityEffect;
        private Caster caster;

        public float CurrentStack { get; set; } = 0;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            threshold.Initialize(modifier);
            range.Initialize(modifier);

            caster = modifier.Target.Entity.GetCachedComponent<Caster>();
            caster.OnAbilityInitialized += OnAbilityInitialized;
        }

        private void OnAbilityInitialized()
        {
            caster.OnAbilityInitialized -= OnAbilityInitialized;

            AbilityAnimationBaseCharacter abilityEntity = caster.Abilities.FirstOrDefault(x => x.Definition == abilityAffected) as AbilityAnimationBaseCharacter;
            abilityEffect = abilityEntity.Effects.FirstOrDefault(x => x is ProjectileAbilityEffect) as ProjectileAbilityEffect;
            abilityEffect.OnProjectileCreated += OnProjectileCreated;
        }

        private void OnProjectileCreated(Projectile.ProjectileEntity projectile)
        {
            CurrentStack++;

            if (CurrentStack >= threshold.GetOrThrow())
            {
                Target target = Target.All.Where(x => x.Entity.IsActive
                        && projectile.Target != x
                        && x.Entity.GetCachedComponent<AgentIdentity>().Faction != modifier.Target.Entity.GetCachedComponent<AgentIdentity>().Faction
                        && Mathf.Abs(modifier.Target.Entity.transform.position.x - x.transform.position.x) < range.Get().Get<float>())
                    .OrderBy(x => x.Entity.GetCachedComponent<AgentIdentity>().Priority)
                    .FirstOrDefault();

                if (target == null)
                {
                    CurrentStack = threshold.GetOrThrow();
                    return;
                }

                GameObject gameObject = UnityEngine.Object.Instantiate(projectile.gameObject, projectile.transform.position, Quaternion.identity);
                ProjectileEntity duplicate = gameObject.GetComponent<ProjectileEntity>();

                duplicate.Initialize(modifier, target, projectile.Faction, projectile.Parameters.Select(x => x.Clone())
                    .Append(new ProjectileParameter<float>("angle", 45))
                    .Append(new ProjectileParameter<Target>("ignore", projectile.Target)).ToArray());

                CurrentStack = 0;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            caster.OnAbilityInitialized -= OnAbilityInitialized;
            abilityEffect.OnProjectileCreated -= OnProjectileCreated;
        }
    }
}
