using System;
using UnityEngine;

namespace Game
{
    public class Base : AgentObject, IBlock
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Collider2D hitbox;

        public Entity Entity { get; set; }
        public float Health { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public float MaxHealth => maxHealth;
        public float Defense => defense;
        public bool IsDead => Health <= 0;

        public event Action<AttackResult, Attackable> OnDamageTaken;

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnDamageTaken += Base_OnDamageTaken;
        }

        private void Base_OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (Health <= 0 && !IsDead)
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

        public bool IsBlocking(Character character)
        {
            return hitbox.IsTouching(character.Hitbox) &&
                character.OriginalFaction != this.OriginalFaction;
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Health = maxHealth;
        }
    }
}

