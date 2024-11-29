using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PeriodicDamagePoolEffect : PoolEffect
    {
        public float Damage { get; set; }

        private float lastTimeApplied = float.MinValue;

        public override void Apply(Pool pool, Target targeteable)
        {
            if (Time.time - lastTimeApplied < 1f)
                return;

            lastTimeApplied = Time.time;
            base.Apply(pool, targeteable);

            if ((targeteable.Entity as AgentObject).Faction == pool.Faction)
                return;

            if (targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            attackable.TakeAttack(pool.GetCachedComponent<AttackFactory>().Generate(Damage, 0, 0, false, false, false, attackable));
        }
    }
}
