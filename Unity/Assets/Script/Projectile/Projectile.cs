using Extension;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        private enum State
        {
            Alive,
            Dead
        }

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeReference, SubclassSelector] private ProjectileDeath projectileDeath = new ProjectileStickDeath();

        public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

        private float damage;
        private Agent source;
        private State state = State.Alive;

        public void Initialize(Agent source, float damage, float angle, Vector3 target)
        {
            this.source = source;
            this.damage = damage;

            SolveForVelocity(this.transform.position, target, Physics2D.gravity.y * rigidbody.gravityScale, angle, out Vector3 velocity);

            rigidbody.velocity = velocity;
        }

        private void Update()
        {
            this.transform.right = rigidbody.velocity;

            if (state == State.Dead)
                projectileDeath.Update(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (state != State.Alive)
                return;

            if (collision.gameObject.CompareTag(GameTag.GROUND))
            {
                Kill(collision.gameObject);
                return;
            }

            if (collision.gameObject.CompareTag(GameTag.HIT_BOX))
            {
                if (collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                    && targeteable.Faction != source.Faction)
                {
                    targeteable.TakeAttack(damage);
                    Kill(collision.gameObject);
                }
            }
        }

        private void Kill(GameObject collision)
        {
            state = State.Dead;

            projectileDeath.Start(this, collision);
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
