using Game.Agent;
using Game.Components;
using Game.EventChannel;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public class BaseEntity : AgentObject
    {
        [SerializeField] private float maxHealth = 1000;

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public float Health { get; set; }
        public float MaxHealth { get => maxHealth; }
        public bool IsDead => Health <= 0;
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
            DeathEventChannel.Instance.Publish(new DeathEventChannel.Event() { AgentObject = this });
            Destroy(gameObject);
        }

        public override void Spawn(AgentEntity agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            StatisticIndex statisticIndex = GetCachedComponent<StatisticIndex>();
            statisticIndex.Add(new StatisticFunction<float>(() => Health, StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Health)));
            statisticIndex.Add(new StatisticFunction<float>(() => MaxHealth, StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.MaxHealth)));

            spawnPoint.Direction = direction;
            Health = MaxHealth;
        }
    }
}

