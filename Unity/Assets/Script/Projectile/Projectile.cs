using Extension;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour, IAttackSource
    {
        private enum State
        {
            Alive,
            Dead
        }

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeReference, SubclassSelector] private ProjectileDeath projectileDeath = new ProjectileStickDeath();
        [SerializeReference, SubclassSelector] private ProjectileMovement projectileMovement = new ProjectileAngledMovement();

        public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

        private Attack attack;
        private Character source;
        private State state = State.Alive;

        public void Initialize(Character source, Attack attack, Vector3 target)
        {
            this.source = source;
            this.attack = attack;

            projectileMovement.Initialize(this, target);
        }

        private void Update()
        {
            projectileMovement.Update();

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
                if (collision.gameObject.TryGetComponentInParent<IAttackable>(out IAttackable attackable)
                    && attackable.Faction != source.Faction)
                {
                    Attack clonedAttack = attack.Clone();
                    clonedAttack.AttackSource.Sources.Add(this);

                    attackable.TakeAttack(clonedAttack);
                    Kill(collision.gameObject);
                }
            }
        }

        private void Kill(GameObject collision)
        {
            state = State.Dead;

            projectileDeath.Start(this, collision);
        }


    }
}
