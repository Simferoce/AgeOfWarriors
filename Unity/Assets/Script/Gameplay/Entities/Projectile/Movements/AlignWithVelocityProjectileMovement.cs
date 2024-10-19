using System;

namespace Game.Projectile
{
    [Serializable]
    public class AlignWithVelocityProjectileMovement : ProjectileMovement
    {
        public override void Update()
        {
            projectile.transform.right = projectile.Rigidbody.linearVelocity;
        }
    }
}
