using Game.Ability;
using Game.Agent;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class StraightProjectileMovement : ProjectileMovement
    {
        [SerializeField] private float speed;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            if (projectile.Parent is not AbilityEntity ability)
            {
                Debug.LogError($"Expecting the parent object to be a {nameof(AbilityEntity)}", projectile);
                return;
            }

            if (ability.Caster.Entity is not AgentObject agentObject)
            {
                Debug.LogError($"Expecting the caster to be a {nameof(AgentObject)}", projectile);
                return;
            }

            Vector3 velocity = Vector3.right * agentObject.Direction * speed;
            projectile.Rigidbody.linearVelocity = velocity;
            projectile.transform.right = projectile.Rigidbody.linearVelocity;
        }

        public override void Update()
        {

        }
    }
}
