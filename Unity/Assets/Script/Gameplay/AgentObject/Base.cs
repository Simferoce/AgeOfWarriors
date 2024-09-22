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
        [SerializeField] private float defense = 5;

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public float Health { get; set; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Defense { get => defense; set => defense = value; }
        public bool IsDead => this.Health <= 0;
        public Entity Entity { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public event Action<AttackResult, Attackable> OnDamageTaken;

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
            yield return new StatisticTemporary<float>(this, "health", Health, StatisticRepository.GetDefinition(StatisticRepository.Health));
            yield return new StatisticTemporary<float>(this, "maxhealth", MaxHealth, StatisticRepository.GetDefinition(StatisticRepository.MaxHealth));
            yield return new StatisticTemporary<float>(this, "defense", Defense, StatisticRepository.GetDefinition(StatisticRepository.Defense));

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Health = MaxHealth;
        }
    }
}

