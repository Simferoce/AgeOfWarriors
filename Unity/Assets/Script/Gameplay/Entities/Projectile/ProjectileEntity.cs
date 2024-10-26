using Game.Components;
using Game.Modifier;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif
using UnityEngine;

namespace Game.Projectile
{
    [RequireComponent(typeof(ModifierHandler))]
    [RequireComponent(typeof(AttackFactory))]
    public partial class ProjectileEntity : Entity
    {
        public enum State
        {
            Alive,
            Dead
        }

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeReference, SubclassSelector] private List<ProjectileMovement> projectileMovements = new List<ProjectileMovement>();
        [SerializeReference, SubclassSelector] private List<ProjectileBehaviour> behaviours = new List<ProjectileBehaviour>();

        public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
        public State StateValue { get => state; set => state = value; }
        public Target Target { get; set; }
        public List<ProjectileMovement> ProjectileMovements { get => projectileMovements; set => projectileMovements = value; }
        public List<ProjectileParameter> Parameters { get => parameters; set => parameters = value; }
        public override FactionType Faction => faction;

        protected FactionType faction;
        private State state = State.Alive;
        private List<ProjectileParameter> parameters;
        private ProjectileDeath projectileDeath = null;

        public void Initialize(Entity source, Target target, FactionType faction, params ProjectileParameter[] parameters)
        {
            this.faction = faction;
            this.Target = target;
            this.parameters = parameters.ToList();
            Parent = source;

            foreach (ProjectileMovement movement in projectileMovements)
                movement.Initialize(this);

            foreach (ProjectileBehaviour behaviour in behaviours)
                behaviour.Initialize(this);
        }

        private void Update()
        {
            foreach (ProjectileMovement context in projectileMovements)
                context.Update();

            if (StateValue == State.Alive)
            {
                foreach (ProjectileBehaviour behaviour in behaviours)
                    behaviour.Update();
            }
            else if (StateValue == State.Dead)
            {
                projectileDeath.Update(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (StateValue != State.Alive)
                return;

            foreach (IProjectileZoneBehaviour effect in behaviours.OfType<IProjectileZoneBehaviour>())
                effect.EnterZone(collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            foreach (IProjectileZoneBehaviour effect in behaviours.OfType<IProjectileZoneBehaviour>())
                effect.LeaveZone(collider);
        }

        public void Kill(ProjectileDeath death)
        {
            StateValue = State.Dead;
            projectileDeath = death;

            if (projectileDeath == null)
                GameObject.Destroy(gameObject);
        }
    }
}
