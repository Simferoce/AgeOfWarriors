using System;
using UnityEngine;

namespace Game
{
    public class Base : AgentObject, IAttackable, ITargeteable, IBlock
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private SpawnPoint spawnPoint;

        [Header("Target")]
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        public float Health { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public Vector3 CenterPosition => transform.position;
        public Vector3 TargetPosition => targetPosition.position;
        public float MaxHealth => maxHealth;
        public float Defense => defense;
        public bool IsDead => Health <= 0;

        public event Action<AttackResult, IAttackable> OnDamageTaken;

        private AttackHandler attackHandler = new AttackHandler();

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
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

        public void TakeAttack(Attack attack)
        {
            AttackHandler.Result result = attackHandler.TakeAttack(attack, new AttackHandler.Input(
                    this,
                    currentHealth: Health,
                    defense: Defense));

            Health -= result.DamageToTake;

            AttackResult attackResult = new AttackResult(attack, result.DamageToTake, result.DefenseDamagePrevented, Health <= 0, this);
            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attackResult);

            OnDamageTaken?.Invoke(attackResult, this);

            if (Health <= 0 && !IsDead)
                Death();
        }
    }
}

