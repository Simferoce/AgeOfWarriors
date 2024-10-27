using Game.Components;
using Game.Modifier;
using UnityEngine;

namespace Game.Agent
{
    [RequireComponent(typeof(ModifierHandler))]
    public abstract class AgentObject : Entity
    {
        public delegate void AttackedLanded(AttackResult attack);

        public override FactionType Faction => Agent.Faction;
        public int Direction { get; protected set; }
        public AgentEntity Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public int Priority { get => SpawnNumber; }
        public string Name => name;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Spawn(AgentEntity agent, int spawnNumber, int direction)
        {
            Direction = direction;
            SpawnNumber = spawnNumber;
            Agent = agent;

            transform.localScale = new Vector3(Mathf.Sign(direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public virtual AgentObjectDefinition GetDefinition() { return null; }
    }

    public abstract class AgentObject<T> : AgentObject
        where T : AgentObjectDefinition
    {
        public T Definition { get; set; }

        public override AgentObjectDefinition GetDefinition()
        {
            return Definition;
        }
    }
}