using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class FixHeightProjectileMovement : ProjectileMovement
    {
        [SerializeField] private float height;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            projectile.Rigidbody.linearVelocity = SolveForVelocity(projectile.transform.position, projectile.Target.TargetPosition, height, Physics2D.gravity.y * projectile.Rigidbody.gravityScale);
        }

        private Vector2 SolveForVelocity(Vector3 startPosition, Vector3 endPosition, float height, float gravity)
        {
            //v_f^2 = v_i^2 + 2 * g * h
            //v_i = sqrt(-2 * g * h)

            //v_f = v_i + g * t
            //t = - v_i / g

            //v_f = d / t
            float velocityY = Mathf.Sqrt(-2 * gravity * height);
            float time = -2 * velocityY / gravity;

            float distance = endPosition.x - startPosition.x;
            float velocityX = distance / time;

            return new Vector2(velocityX, velocityY);
        }

        public override void Update()
        {
        }
    }
}
