using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public abstract class OnHitProjectileBehaviourTargetProvider
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public abstract Entity Execute(AttackResult attackResult);
    }
}
