using Game.Agent;
using Game.Components;
using Game.Modifier;
using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Pool
{
    [RequireComponent(typeof(AttackFactory))]
    [RequireComponent(typeof(ModifierApplier))]
    public class PoolEntity : Entity
    {
        [SerializeField] private Collider2D hitbox;
        [SerializeReference, SubclassSelector] private List<PoolEffect> poolEffects;

        private float startTime;

        public void Initialize(Entity parent)
        {
            base.Initialize();
            Parent = parent;
            startTime = Time.time;

            foreach (PoolEffect poolEffect in poolEffects)
                poolEffect.Initialize(this);

            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => GetCachedComponent<AgentIdentity>().Faction));
        }

        private void FixedUpdate()
        {
            foreach (Target target in Target.All)
            {
                if (!target.enabled)
                    continue;

                if (!hitbox.OverlapPoint(target.CenterPosition))
                    continue;

                foreach (PoolEffect poolEffect in poolEffects)
                    poolEffect.Apply(this, target);
            }

            if (Time.time - startTime > this["duration"])
            {
                Destroy(gameObject);
                return;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (PoolEffect poolEffect in poolEffects)
                poolEffect.Dispose();
        }

        public T GetEffect<T>()
            where T : PoolEffect
        {
            PoolEffect poolEffect = poolEffects.FirstOrDefault(x => x is T);
            Assert.IsNotNull(poolEffect, "Attempting to get an effect that does not exists in the pool.");
            return (T)poolEffect;
        }

        public void AddPoolEffect(PoolEffect poolEffect)
        {
            poolEffect.Initialize(this);
            poolEffects.Add(poolEffect);
        }
    }
}
