using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class Pool : AgentObject
    {
        [SerializeField] private Collider2D hitbox;
        [SerializeReference, SubclassSelector] private List<PoolEffect> poolEffects;

        public float Duration { get; set; }

        private float startTime;

        public void Initialize()
        {
            startTime = Time.time;

            foreach (PoolEffect poolEffect in poolEffects)
                poolEffect.Initialize(this);
        }

        private void FixedUpdate()
        {
            foreach (AgentObject agent in AgentObject.All)
            {
                if (!agent.IsActive)
                    continue;

                if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                    continue;

                if (!hitbox.OverlapPoint(targeteable.CenterPosition))
                    continue;

                foreach (PoolEffect poolEffect in poolEffects)
                    poolEffect.Apply(this, targeteable);
            }

            if (Time.time - startTime > Duration)
            {
                GameObject.Destroy(gameObject);
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
