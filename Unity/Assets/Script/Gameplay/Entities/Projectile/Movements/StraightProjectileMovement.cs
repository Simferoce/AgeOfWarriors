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

            if (!(projectile.Parent is AgentObject agentObject))
                throw new ArgumentException($"Expecting the parent object to be an {nameof(AgentObject)}");

            Vector3 velocity = Vector3.right * agentObject.Direction * speed;
            projectile.Rigidbody.linearVelocity = velocity;
            projectile.transform.right = projectile.Rigidbody.linearVelocity;
        }

        public override void Update()
        {

        }
    }
}
