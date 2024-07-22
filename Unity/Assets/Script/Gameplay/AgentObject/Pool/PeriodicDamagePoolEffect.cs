using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PeriodicDamagePoolEffect : PoolEffect
    {
        public float Damage { get; set; }

        private float lastTimeApplied = float.MinValue;

        public override void Apply(Pool pool, ITargeteable targeteable)
        {
            if (Time.time - lastTimeApplied < 1f)
                return;

            lastTimeApplied = Time.time;
            base.Apply(pool, targeteable);

            if (targeteable.Faction == pool.Faction)
                return;

            if (!targeteable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                return;

            attackable.TakeAttack(new Attack(new AttackSource(pool), Damage, 0, 0, false, false, false));
        }
    }
}
