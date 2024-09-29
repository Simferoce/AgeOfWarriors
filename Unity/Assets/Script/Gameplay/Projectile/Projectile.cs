using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierHandler))]
    [RequireComponent(typeof(AttackFactory))]
    public partial class Projectile : Entity
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
        public State StateValue { get => state; set => state = value; }
        public Target Target { get => target; set => target = value; }
        public Target Ignore { get; set; }
        public List<ProjectileMovement> ProjectileMovements { get => projectileMovements; set => projectileMovements = value; }
        public event Impacted OnImpacted;
        public override Faction Faction => faction;

        protected Faction faction;
        private Target target;
        private State state = State.Alive;

        public void Initialize(Entity source, Target target, Faction faction)
        {
            this.faction = faction;
            this.target = target;
            this.Parent = source;

            foreach (ProjectileMovement movement in projectileMovements)
                movement.Initialize(this);

            foreach (ProjectileImpact effect in impacts)
                effect.Initialize(this);

            //foreach (IProjectileModifier projectileModifier in agentObject.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<IProjectileModifier>().Where(x => x.HasModifier))
            //{
            //    this.Entity.GetCachedComponent<ModifierHandler>().AddModifier(projectileModifier.GetModifier(this));
            //}
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
