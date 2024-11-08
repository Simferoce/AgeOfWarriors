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

            projectile.Initialize(Ability, Ability.Targets[0], Ability.Faction, parameters.Select(x => x.Create(Ability)).ToArray());
        }
    }
}
