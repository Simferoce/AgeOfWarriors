using UnityEngine;

namespace Game
{
    public class Pool : AgentObject, IAttackSource
    {
        [SerializeField] private Collider2D hitbox;

        public float Duration { get; set; }
        public float Damage { get; set; }

        private float lastEffectApplied;
        private float startTime;

        private void Start()
        {
            lastEffectApplied = Time.time;
            startTime = Time.time;
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

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (agent.Faction == this.Faction)
                        continue;

                    if (!hitbox.OverlapPoint(targeteable.CenterPosition))
                        continue;

                    attackable.TakeAttack(new Attack(new AttackSource(this), Damage, 0, 0, false, false, false));
                }

                lastEffectApplied = Time.time;
            }

            if (Time.time - startTime > Duration)
            {
                GameObject.Destroy(gameObject);
                return;
            }
        }
    }
}
