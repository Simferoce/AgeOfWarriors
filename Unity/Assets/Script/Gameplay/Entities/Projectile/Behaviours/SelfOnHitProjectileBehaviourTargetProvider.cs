using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public class SelfOnHitProjectileBehaviourTargetProvider : OnHitProjectileBehaviourTargetProvider
    {
        public override Entity Execute(AttackResult attackResult)
        {
            return projectile;
        }
    }
}
