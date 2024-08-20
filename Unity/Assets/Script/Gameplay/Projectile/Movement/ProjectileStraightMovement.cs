using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileStraightMovement : ProjectileMovement
    {
        [SerializeField] private float speed;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);

            Vector3 velocity = Vector3.right * projectile.Caster.Direction * speed;
            projectile.Rigidbody.velocity = velocity;
            projectile.transform.right = projectile.Rigidbody.velocity;
        }

        public override void Update()
        {

        }
    }
}
