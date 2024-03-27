using UnityEngine;

namespace Game
{
    public class Wall : AgentObject, ITargeteable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private Transform targetPosition;

        public Faction Faction => Agent.Faction;
        public int Priority => int.MaxValue;
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public Vector3 Position => targetPosition.position;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeAttack(float damage)
        {
            health -= damage;
            if (health <= 0)
                Destroy(this.gameObject);
        }

        public bool Attackable()
        {
            return this.health > 0;
        }

        public bool CanBlocks(Faction faction)
        {
            return faction != this.Faction;
        }
    }
}