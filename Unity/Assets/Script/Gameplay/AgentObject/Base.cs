using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public class Base : AgentObject
    {
        [SerializeField] private float maxHealth = 1000;

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public float Health { get => health; set => health.Modify(health); }
        public float MaxHealth { get => health.Max; }
        public bool IsDead => this.Health <= 0;
        public Entity Entity { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public event Action<AttackResult, Attackable> OnDamageTaken;

        private StatisticFloatModifiable health;

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnDamageTaken += Base_OnDamageTaken;
        }

        private void Base_OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (IsDead)
                Death();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCachedComponent<Attackable>().OnDamageTaken -= Base_OnDamageTaken;
        }

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            Destroy(this.gameObject);
        }

        public override IEnumerable<Statistic> GetStatistic()
        {
            yield return health;

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            health = new StatisticFloatModifiable("health", StatisticRepository.Health, new StatisticSerialize<float>("max", StatisticRepository.MaxHealth, maxHealth));
            health.Initialize(this);

            Health = MaxHealth;
        }
    }
}

