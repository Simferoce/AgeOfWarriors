﻿using Game.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
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

        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public virtual bool Validate(ProjectileEntity projectile) { return false; }
        public abstract ImpactReport Impact(GameObject collision);
        public virtual void LeaveZone(GameObject collision) { }
        public virtual ImpactReport Update() { return new ImpactReport(ImpactStatus.NotImpacted); }
    }
}