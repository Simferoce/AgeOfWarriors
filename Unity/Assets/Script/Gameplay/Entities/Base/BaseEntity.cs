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
    public class BaseEntity : Entity
    {
        [SerializeField] private float maxHealth = 1000;

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public float Health { get; set; }
        public float MaxHealth { get => maxHealth; }
        public bool IsDead => Health <= 0;
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public event Action<AttackResult, Attackable> OnDamageTaken;

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnDamageTaken += Base_OnDamageTaken;
            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => GetCachedComponent<AgentIdentity>().Faction));
        }

        private void Base_OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (IsDead)
                Death();
        }

        public void Death()
        {
            DeathEventChannel.Instance.Publish(new DeathEventChannel.Event() { Entity = this });

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

