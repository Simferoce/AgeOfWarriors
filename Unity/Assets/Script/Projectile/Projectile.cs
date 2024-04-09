using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour, IAttackSource
    {
        public enum State
        {
            Alive,
            Dead
        }

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeReference, SubclassSelector] private ProjectileDeath projectileDeath = new ProjectileStickDeath();
        [SerializeReference, SubclassSelector] private List<ProjectileMovement> projectileMovements = new List<ProjectileMovement>();
        [SerializeReference, SubclassSelector] private List<ProjectileImpact> impacts = new List<ProjectileImpact>();

        public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
        public List<ProjectileContext> Contexts { get => contexts; set => contexts = value; }
        public Character Character { get => character; set => character = value; }
        public State StateValue { get => state; set => state = value; }

        private List<ProjectileContext> contexts;
        private Character character;
        private State state = State.Alive;

        public void Initialize(Character character, List<ProjectileContext> contexts)
        {
            this.character = character;
            this.contexts = contexts;

            foreach (ProjectileMovement context in projectileMovements)
                context.Initialize(this);

            foreach (ProjectileImpact effect in impacts)
                effect.Initialize(this);
        }

        private void Update()
        {
            foreach (ProjectileMovement context in projectileMovements)
                context.Update();

            if (StateValue == State.Alive)
            {
                bool impacted = false;
                foreach (ProjectileImpact effect in impacts)
                    impacted |= effect.Update();

                if (impacted)
                    Kill(null);
            }
            else if (StateValue == State.Dead)
            {
                projectileDeath.Update(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (StateValue != State.Alive)
                return;

            bool impacted = false;
            foreach (ProjectileImpact effect in impacts)
                impacted |= effect.Impact(collision.gameObject);

            if (impacted)
                Kill(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            foreach (ProjectileImpact effect in impacts)
                effect.LeaveZone(collision.gameObject);
        }

        private void Kill(GameObject collision)
        {
            StateValue = State.Dead;

            projectileDeath.Start(this, collision);
        }
    }
}
