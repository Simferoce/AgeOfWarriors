using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Base : AgentObject, IBlocker, IModifiable, IAttackable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Transform targetPosition;
        [SerializeField] private Collider2D hitbox;

        public event Action<Attack, IAttackable> OnDamageTaken;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public Faction Faction => Agent.Faction;
        public int Priority => int.MaxValue;
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public Vector3 TargetPosition => transform.position;
        public ModifierHandler ModifierHandler { get; set; } = new ModifierHandler();
        public Collider2D Collider { get => hitbox; }
        public bool IsActive { get => true; }

        protected override void Awake()
        {
            base.Awake();
            health = maxHealth;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ModifierHandler.Dispose();
        }

        public bool Attackable()
        {
            return this.health > 0;
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return Collider.ClosestPoint(this.TargetPosition);
        }

        public bool IsBlocking(Faction faction)
        {
            return faction != this.Faction;
        }

        public void TakeAttack(Attack attack)
        {
            health -= attack.Damage;

            OnDamageTaken?.Invoke(attack, this);
        }

        public void AddModifier(Modifier modifier)
        {
            ModifierHandler.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            ModifierHandler.Remove(modifier);
        }

        public List<Modifier> GetModifiers()
        {
            return ModifierHandler.Modifiers;
        }
    }
}

