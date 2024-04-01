using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform origin;

        [Header("Statistics")]
        [SerializeField] private float armorPenetration;

        public override void Apply()
        {
            IAttackable target = character.GetTarget();

            if (target != null)
            {
                GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
                Projectile projectile = gameObject.GetComponent<Projectile>();

                projectile.Initialize(character, new Attack(new AttackSource(character), character.AttackPower, armorPenetration), target.TargetPosition);
            }
        }
    }
}
