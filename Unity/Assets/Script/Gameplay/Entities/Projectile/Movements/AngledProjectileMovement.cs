using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class AngledProjectileMovement : ProjectileMovement
    {
        [SerializeField] private float angle;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            ProjectileParameter<float> projectileParameter = projectile.Parameters.FirstOrDefault(x => x.Name == "angle") as ProjectileParameter<float>;
            if (projectileParameter != null)
                angle = projectileParameter.GetValue();

            SolveForVelocity(projectile.transform.position, projectile.Target.TargetPosition, Physics2D.gravity.y * projectile.Rigidbody.gravityScale, angle, out Vector3 velocity);

            if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y))
                return;

            projectile.Rigidbody.linearVelocity = velocity;
        }

        public override void Update()
        {

        }

        private void SolveForVelocity(Vector3 startPosition, Vector3 endPosition, float gravity, float angle, out Vector3 velocity)
        {
            //x = v * t

            //x = v * cos(a) * t
            //h = v * sin(a) * t + 1 / 2 * g * t ^ 2

            //t = x / (cos(a) * v)
            //h = v * sin(a) * (x / (cos(a) * v)) + 1 / 2 * g * (x / (cos(a) * v)) ^ 2
            //h = v * sin(a) * (x / (cos(a) * v)) + 1 / 2 * g * (x / (cos(a) * v)) * (x / (cos(a) * v))
            //h = v * sin(a) * (x / (cos(a) * v)) + 1 / 2 * g * x ^ 2 / (cos(a) ^ 2 * v ^ 2)
            //h = sin(a) * (x / cos(a)) + 1 / 2 * g * x ^ 2 / (cos(a) ^ 2 * v ^ 2)
            //h - sin(a) * (x / cos(a)) = 1 / 2 * g * x ^ 2 / (cos(a) ^ 2 * v ^ 2)
            //1 / 2 * g * x ^ 2 / (h - sin(a) * (x / cos(a))) = cos(a) ^ 2 * v ^ 2
            //1 / 2 * g * x ^ 2 / (h - sin(a) * (x / cos(a))) / cos(a) ^ 2 = v ^ 2

            Vector3 delta = endPosition - startPosition;

            float x = Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z);
            float x2 = x * x;
            float h = delta.y;
            float g = gravity;
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * angle);
            float cos2 = cos * cos;

            float r = Mathf.Sqrt((0.5f * g * x2) / (h * cos2 - sin * x * cos));

            Vector2 planeDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            Vector3 direction = new Vector3(delta.x, 0, delta.z).normalized * planeDirection.x + Vector3.up * planeDirection.y;
            velocity = direction * r;
        }
    }
}
