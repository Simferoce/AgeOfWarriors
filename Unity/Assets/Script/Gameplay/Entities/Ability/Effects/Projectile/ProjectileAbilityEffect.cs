using Game.Agent;
using Game.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;
        [SerializeReference, SubclassSelector] private List<ProjectileParameterFactory> parameters;

        public event System.Action<ProjectileEntity> OnProjectileCreated;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);

            foreach (ProjectileParameterFactory parameter in parameters)
                parameter.Initialize(ability);
        }

        public override void Apply()
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(projectilePrefab, origin.GetPosition(Ability), Quaternion.identity);
            ProjectileEntity projectile = gameObject.GetComponent<ProjectileEntity>();

            float direction = Ability.Caster.Entity.GetCachedComponent<AgentIdentity>().Direction;
            projectile.Initialize(Ability, Ability.Targets.Count > 0 ? Ability.Targets[0] : null, Ability.Faction, parameters.Select(x => x.Create(Ability)).Append(new ProjectileParameter<float>("direction", direction)).ToArray());

            OnProjectileCreated?.Invoke(projectile);
        }
    }
}
