using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierHandler))]
    [StatisticObject("projectile")]
    public class Projectile : CachedMonobehaviour, IAttackSource, IContext
    {
        public delegate void Impacted(List<ITargeteable> targeteables);

        public enum State
        {
            Alive,
            Dead
        }

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeReference, SubclassSelector] private ProjectileDeath projectileDeath = new ProjectileStickDeath();
        [SerializeReference, SubclassSelector] private List<ProjectileMovement> projectileMovements = new List<ProjectileMovement>();
        [SerializeReference, SubclassSelector] private List<ProjectileImpact> impacts = new List<ProjectileImpact>();

        [Statistic()] public List<IContext> Context { get => context; set => context = value; }
        public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
        public ICaster Caster { get => caster; set => caster = value; }
        public State StateValue { get => state; set => state = value; }
        public ITargeteable Target { get => target; set => target = value; }
        public ITargeteable Ignore { get; set; }
        public List<ProjectileMovement> ProjectileMovements { get => projectileMovements; set => projectileMovements = value; }
        public event Impacted OnImpacted;
        public Faction Faction { get; set; }

        private ICaster caster;
        private List<IContext> context;
        private ITargeteable target;
        private State state = State.Alive;

        public void Initialize(ICaster caster, ITargeteable target, Faction faction, params IContext[] context)
        {
            this.Initialize(caster, target, faction, context.ToList());
        }

        public void Initialize(ICaster caster, ITargeteable target, Faction faction, List<IContext> context)
        {
            this.Faction = faction;
            this.caster = caster;
            this.context = context;
            this.target = target;

            Ownership.SetOwner(this, caster);

            foreach (ProjectileMovement movement in projectileMovements)
                movement.Initialize(this);

            foreach (ProjectileImpact effect in impacts)
                effect.Initialize(this);

            foreach (IProjectileModifier projectileModifier in caster.GetCachedComponent<IModifiable>().GetModifiers().OfType<IProjectileModifier>().Where(x => x.HasModifier))
            {
                this.GetCachedComponent<IModifiable>().AddModifier(projectileModifier.GetModifier(this));
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
