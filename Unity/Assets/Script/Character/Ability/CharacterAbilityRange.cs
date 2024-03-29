using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterAbilityRange : CharacterAbility
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float angle = 30f;
        [SerializeField] private Transform origin;

        protected override void OnAnimatorEventAbilityUsed()
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
