using Game.Agent;
using Game.Components;
using System;
using UnityEngine;

namespace Game.Pool
{
    [Serializable]
    public class PeriodicDamagePoolEffect : PoolEffect
    {
        private float lastTimeApplied = float.MinValue;

        public override void Apply(PoolEntity pool, Target targeteable)
        {
            if (Time.time - lastTimeApplied < 1f)
                return;

            lastTimeApplied = Time.time;
            base.Apply(pool, targeteable);

            if (targeteable.Entity.GetCachedComponent<AgentIdentity>().Faction == pool.GetCachedComponent<AgentIdentity>().Faction)
                return;

            if (!targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            AttackFactory attackFactory = pool.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: 0.0f
                );

            attackable.TakeAttack(attack);
        }
    }
}
