using Game.Agent;
using Game.Components;
using Game.Modifier;
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

        public float Duration { get; set; }

        private float startTime;

        public void Initialize(Entity parent, FactionType faction)
        {
            Parent = parent;
            startTime = Time.time;

            foreach (PoolEffect poolEffect in poolEffects)
                poolEffect.Initialize(this);
        }

        private void FixedUpdate()
        {
            foreach (AgentObject agent in EntityRepository.Instance.GetByType<AgentObject>())
            {
                if (!agent.TryGetCachedComponent(out Target targeteable))
                    continue;

                if (!targeteable.enabled)
                    continue;

                if (!hitbox.OverlapPoint(targeteable.CenterPosition))
                    continue;

                foreach (PoolEffect poolEffect in poolEffects)
                    poolEffect.Apply(this, targeteable);
            }

            if (Time.time - startTime > Duration)
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
