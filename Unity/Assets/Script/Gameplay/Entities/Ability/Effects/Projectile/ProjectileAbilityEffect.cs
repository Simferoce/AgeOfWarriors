using Game.Projectile;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;

        public override void Apply()
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(projectilePrefab, origin.GetPosition(Ability), Quaternion.identity);
            ProjectileEntity projectile = gameObject.GetComponent<ProjectileEntity>();

            projectile.Initialize(Ability, Ability.Targets[0], Ability.Faction);
        }
    }
}
