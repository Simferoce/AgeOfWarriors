using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float angle = 30f;
        [SerializeField] private Transform origin;

        public override void Apply(Character character)
        {
            ITargeteable target = character.GetTarget();

            if (target != null)
            {
                GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
                Projectile projectile = gameObject.GetComponent<Projectile>();

                projectile.Initialize(character.Agent, character.AttackPower, angle, target.Position);
            }
        }
    }
}
