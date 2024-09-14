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
            public List<Target> ImpactedTargeteables { get; set; }

            public ImpactReport(ImpactStatus impactStatus)
            {
                ImpactStatus = impactStatus;
                ImpactedTargeteables = new List<Target>();
            }

            public ImpactReport(ImpactStatus impactStatus, List<Target> impactedTargeteable) : this(impactStatus)
            {
                ImpactedTargeteables = impactedTargeteable;
            }

            public ImpactReport(ImpactStatus impactStatus, Target impactedTargeteable) : this(impactStatus)
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
