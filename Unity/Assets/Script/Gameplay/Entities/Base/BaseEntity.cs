using Game.Agent;
using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public class BaseEntity : Entity
    {
        [SerializeField] private float maxHealth = 1000;

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public float Health { get => this[StatisticDefinitionRegistry.Instance.Health]; set => this[StatisticDefinitionRegistry.Instance.Health].Set(value); }
        public float MaxHealth => this[StatisticDefinitionRegistry.Instance.MaxHealth];
        public bool IsDead => Health <= 0;
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public event Action<AttackResult, Attackable> OnDamageTaken;

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnDamageTaken += Base_OnDamageTaken;
            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => GetCachedComponent<AgentIdentity>().Faction));

            StatisticRepository.Add(new StatisticFloat("health", StatisticDefinitionRegistry.Instance.Health, maxHealth));
            StatisticRepository.Add(new StatisticFloat("max_health", StatisticDefinitionRegistry.Instance.MaxHealth, maxHealth));
        }

        private void Base_OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (IsDead)
                Death();
        }

        public void Death()
        {
            DeathEventChannel.Global.Publish(new DeathEventChannel.Event() { Entity = this });

            GetCachedComponent<Attackable>().OnDamageTaken -= Base_OnDamageTaken;
            Destroy(gameObject);
        }

        public override void Initialize()
        {
            spawnPoint.Direction = GetCachedComponent<AgentIdentity>().Direction;
            Health = MaxHealth;
        }
    }
}

