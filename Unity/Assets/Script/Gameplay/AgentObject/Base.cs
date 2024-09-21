using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public class Base : AgentObject
    {
        [SerializeField] public StatisticFloatModifiable Health = new StatisticFloatModifiable("health", StatisticRepository.Health, new StatisticSerialize<float>("max", StatisticRepository.MaxHealth, 1000f));
        [SerializeField] public StatisticSerialize<float> Defense = new StatisticSerialize<float>("defense", StatisticRepository.Defense, 0);
        [SerializeField] public StatisticFunction<Base, bool> IsDead = new StatisticFunction<Base, bool>("isDead", null, x => x.Health.GetValueOrThrow<float>(x) <= 0);

        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

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
            Health.Modify(this, Health.GetValueOrThrow<float>(this) - attackResult.DamageTaken);
            if (IsDead.GetValueOrThrow<bool>(this))
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

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Health.Modify(this, Health.Max.GetValueOrThrow<float>(this));
        }
    }
}

