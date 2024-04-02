using System;

namespace Game
{
    [Serializable]
    public class ProjectileAlignWithVelocityMovement : ProjectileMovement
    {
        public override void Update()
        {
            projectile.transform.right = projectile.Rigidbody.velocity;
        }
    }
}
