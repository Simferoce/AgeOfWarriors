using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileFixVelocityMovement : ProjectileMovement
    {
        [SerializeField] private float velocity = 2f;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);

            ProjectileTargetContext context = (ProjectileTargetContext)projectile.Contexts.FirstOrDefault(x => x is ProjectileTargetContext);

            SolveForAngle(projectile.transform.position, context.Target.TargetPosition, Physics2D.gravity.y * projectile.Rigidbody.gravityScale, velocity, out float angle);
            projectile.Rigidbody.velocity = new Vector2(Mathf.Sign(context.Target.TargetPosition.x - projectile.transform.position.x) * Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * velocity;
        }

        public override void Update()
        {

        }

        private bool SolveForAngle(Vector3 startPosition, Vector3 endPosition, float gravity, float velocity, out float angle)
        {
            //x = v * t

            //x = v * cos(a) * t
            //y = y_0 + v * sin(a) * t + 1 / 2 * g * t ^ 2

            //t = x / (v * cos(a))
            //y = y_0 + v * sin(a) * (x / (v * cos(a))) + 1 / 2 * g * (x / (v * cos(a))) ^ 2

            //y = y_0 + x * (sin(a) / cos(a)) + g * (x ^ 2 / (2 * v ^ 2 * cos(a) ^ 2))
            //y = y_0 + x * (sin(a) / cos(a)) + g * (x ^ 2 / (2 * v ^ 2 * cos(a) ^ 2))
            //0 = y_0 - y + x * (sin(a) / cos(a)) + g * (x ^ 2 / (2 * v ^ 2 * cos(a) ^ 2))
            //0 = (y_0 - y) * cos(a) ^ 2 + x * sin(a) * cos(a) + g * x ^ 2 / (2 * v ^ 2)
            //0 = (y_0 - y) * (1 - sin(a)^2) + x * sin(a) * cos(a) + g * x ^ 2 / (2 * v ^ 2)
            //0 = (y_0 - y) - (y_0 - y) * sin(a) ^ 2 + x * sin(a) * cos(a) + g * x ^ 2 / (2 * v ^ 2)
            //0 = - (y_0 - y) * sin(a) ^ 2 + x * sin(a) * cos(a) + (y_0 - y) + g * x ^ 2 / (2 * v ^ 2)
            //0 = - (y_0 - y) / (2 * (1 - cos(2a))) + x / 2 * sin(2a) + (y_0 - y) + g * x ^ 2 / (2 * v ^ 2)
            //0 = (y_0 - y) / (2 * cos(2a)) + x / 2 * sin(2a) + (y_0 - y) / 2 + g * x ^ 2 / (2 * v ^ 2)
            //0 = (y_0 - y) * cos(2a) + x * sin(2a) + (y_0 - y) + g * x ^ 2 / (v ^ 2)
            //0 = sqrt((y_0 - y)^2 + x^2) * cos(2a - arctan(x / (y_0 - y))) + (y_0 - y) + g * x ^ 2 / (v ^ 2)
            //sqrt((y_0 - y)^2 + x^2) * cos(2a - arctan(x / (y_0 - y))) = - (y_0 - y) - g * x ^ 2 / (v ^ 2)
            //cos(2a - arctan(x / (y_0 - y))) = (- (y_0 - y) - g * x ^ 2 / (v ^ 2)) / sqrt((y_0 - y)^2 + x^2)
            //2a - arctan(x / (y_0 - y)) = arccos((- (y_0 - y) - g * x ^ 2 / (v ^ 2)) / sqrt((y_0 - y)^2 + x^2))
            //2a = arccos((- (y_0 - y) - g * x ^ 2 / (v ^ 2)) / sqrt((y_0 - y)^2 + x^2)) + arctan(x / (y_0 - y))
            //a = (arccos((- (y_0 - y) - g * x ^ 2 / (v ^ 2)) / sqrt((y_0 - y)^2 + x^2)) + arctan(x / (y_0 - y))) / 2

            float deltaX = (new Vector3(endPosition.x, 0, endPosition.z) - new Vector3(startPosition.x, 0, startPosition.z)).magnitude;
            float deltaY = startPosition.y - endPosition.y;
            float deltaX2 = deltaX * deltaX;
            float deltaY2 = deltaY * deltaY;
            float velocity2 = velocity * velocity;

            float a = (-deltaY - gravity * deltaX2 / velocity2) / Mathf.Sqrt(deltaY2 + deltaX2);

            //Over
            angle = (Mathf.Acos(Mathf.Clamp(a, -1, 1)) * Mathf.Rad2Deg + Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg) / 2;
            //Under
            //angle = (-Mathf.Acos(Mathf.Clamp(a, -1, 1)) * Mathf.Rad2Deg + Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg) / 2;

            return a < 1 && a > -1;
        }
    }
}
