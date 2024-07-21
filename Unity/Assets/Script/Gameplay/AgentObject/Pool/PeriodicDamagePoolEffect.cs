using System;

namespace Game
{
    [Serializable]
    public class PeriodicDamagePoolEffect : PoolEffect
    {
        public float Damage { get; set; }

        public override void Apply(Pool pool, ITargeteable targeteable)
        {
            base.Apply(pool, targeteable);

            if (targeteable.Faction == pool.Faction)
                return;

            if (!targeteable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                return;

            attackable.TakeAttack(new Attack(new AttackSource(pool), Damage, 0, 0, false, false, false));
        }
    }
}
