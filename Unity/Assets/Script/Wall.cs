using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Wall : AgentObject, ITargeteable, IModifiable, IAttackable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private Transform targetPosition;

        public event Action<Attack, IAttackable> OnDamageTaken;

        public Faction Faction => Agent.Faction;
        public int Priority => int.MaxValue;
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public Vector3 Position => targetPosition.position;
        public ModifierHandler ModifierHandler { get; set; } = new ModifierHandler();

        protected override void Awake()
        {
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

        public bool CanBlocks(Faction faction)
        {
            return faction != this.Faction;
        }

        public void TakeAttack(Attack attack)
        {
            health -= attack.Damage;

            OnDamageTaken?.Invoke(attack, this);

            if (health <= 0)
                Destroy(this.gameObject);
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