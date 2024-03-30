using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileStraightMovement : ProjectileMovement
    {
        [SerializeField] private float speed;

        public override void Initialize(Projectile projectile, Vector3 target)
        {
            base.Initialize(projectile, target);

            Vector3 velocity = (target - projectile.transform.position).XY().normalized * speed;
            projectile.Rigidbody.velocity = velocity;
            projectile.transform.right = projectile.Rigidbody.velocity;
        }

        public override void Update()
        {

        }
    }
}
