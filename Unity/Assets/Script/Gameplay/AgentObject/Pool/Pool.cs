using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class Pool : AgentObject, IAttackSource
    {
        [SerializeField] private Collider2D hitbox;
        [SerializeReference, SubclassSelector] private List<PoolEffect> poolEffects;

        public float Duration { get; set; }

        private float lastEffectApplied;
        private float startTime;
        private AgentObject owner;

        public void Initialize(AgentObject owner)
        {
            this.owner = owner;
            lastEffectApplied = Time.time;
            startTime = Time.time;

            foreach (PoolEffect poolEffect in poolEffects)
                poolEffect.Initialize(this);

            if (owner is Character character)
                character.AddChildEntity(this);
        }

        private void FixedUpdate()
        {
            if (Time.time - lastEffectApplied > 1)
            {
                foreach (AgentObject agent in AgentObject.All)
                {
                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!hitbox.OverlapPoint(targeteable.CenterPosition))
                        continue;

                    foreach (PoolEffect poolEffect in poolEffects)
                        poolEffect.Apply(this, targeteable);
                }

                lastEffectApplied = Time.time;
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

            if (owner is Character character)
                character.RemoveChildEntity(this);
        }

        public T GetEffect<T>()
            where T : PoolEffect
        {
            PoolEffect poolEffect = poolEffects.FirstOrDefault(x => x is T);
            Assert.IsNotNull(poolEffect, "Attempting to get an effect that does not exists in the pool.");
            return (T)poolEffect;
        }
    }
}
