using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierHandler))]
    public partial class Projectile : Entity, IAttackSource
    {
        public delegate void Impacted(List<Target> targeteables);

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
        public AgentObject AgentObject { get => agentObject; set => agentObject = value; }
        public State StateValue { get => state; set => state = value; }
        public Target Target { get => target; set => target = value; }
        public Target Ignore { get; set; }
        public List<ProjectileMovement> ProjectileMovements { get => projectileMovements; set => projectileMovements = value; }
        public event Impacted OnImpacted;
        public Faction Faction { get; set; }
        public List<object> Parameters { get; set; }
        public Entity Entity { get; set; }

        private AgentObject agentObject;
        private Target target;
        private State state = State.Alive;

        public void Initialize(AgentObject agentObject, Target target, Faction faction, params object[] paramters)
        {
            this.Faction = faction;
            this.agentObject = agentObject;
            this.target = target;
            this.Parameters = paramters.ToList();

            //Ownership.SetOwner(this, agentObject);
            throw new System.Exception("Ownership not implemented");

            foreach (ProjectileMovement movement in projectileMovements)
                movement.Initialize(this);

            foreach (ProjectileImpact effect in impacts)
                effect.Initialize(this);

            foreach (IProjectileModifier projectileModifier in agentObject.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<IProjectileModifier>().Where(x => x.HasModifier))
            {
                this.Entity.GetCachedComponent<ModifierHandler>().AddModifier(projectileModifier.GetModifier(this));
            }
        }

        private void Update()
        {
            foreach (ProjectileMovement context in projectileMovements)
                context.Update();

            if (StateValue == State.Alive)
            {
                bool impacted = false;
                foreach (ProjectileImpact effect in impacts)
                {
                    ProjectileImpact.ImpactReport impactReport = effect.Update();
                    if (impactReport.ImpactStatus == ProjectileImpact.ImpactStatus.Impacted)
                    {
                        impacted = true;
                        OnImpacted?.Invoke(impactReport.ImpactedTargeteables);
                    }
                }

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

            foreach (ProjectileImpact effect in impacts)
            {
                ProjectileImpact.ImpactReport impactReport = effect.Impact(collision.gameObject);
                if (impactReport.ImpactStatus == ProjectileImpact.ImpactStatus.Impacted)
                {
                    OnImpacted?.Invoke(impactReport.ImpactedTargeteables);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            foreach (ProjectileImpact effect in impacts)
                effect.LeaveZone(collision.gameObject);
        }

        public void Kill(GameObject collision)
        {
            StateValue = State.Dead;

            projectileDeath.Start(this, collision);
        }
    }
}
