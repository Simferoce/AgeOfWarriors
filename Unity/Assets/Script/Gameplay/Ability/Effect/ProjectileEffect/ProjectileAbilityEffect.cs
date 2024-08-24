using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;

        public override void Apply()
        {
            GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.GetPosition(Ability), Quaternion.identity);
            Projectile projectile = gameObject.GetComponent<Projectile>();

            projectile.Initialize(Ability.Caster.AgentObject, Ability.Targets[0], Ability.FactionWhenUsed, Ability);
        }
    }
}
