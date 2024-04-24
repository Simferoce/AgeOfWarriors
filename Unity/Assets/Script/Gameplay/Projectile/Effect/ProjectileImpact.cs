using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileImpact
    {
        public enum ImpactStatus
        {
            Impacted,
            NotImpacted
        }

        public struct ImpactReport
        {
            public ImpactStatus ImpactStatus { get; set; }
            public List<ITargeteable> ImpactedTargeteables { get; set; }

            public ImpactReport(ImpactStatus impactStatus)
            {
                ImpactStatus = impactStatus;
                ImpactedTargeteables = new List<ITargeteable>();
            }

            public ImpactReport(ImpactStatus impactStatus, List<ITargeteable> impactedTargeteable) : this(impactStatus)
            {
                ImpactedTargeteables = impactedTargeteable;
            }

            public ImpactReport(ImpactStatus impactStatus, ITargeteable impactedTargeteable) : this(impactStatus)
            {
                ImpactedTargeteables.Add(impactedTargeteable);
            }
        }

        protected Projectile projectile;

        public virtual void Initialize(Projectile projectile)
        {
            this.projectile = projectile;
        }

        public abstract ImpactReport Impact(GameObject collision);
        public virtual void LeaveZone(GameObject collision) { }
        public virtual ImpactReport Update() { return new ImpactReport(ImpactStatus.NotImpacted); }
    }
}
