using Game.Components;
using Game.Modifier;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent
{
    [RequireComponent(typeof(ModifierHandler))]
    public abstract class AgentObject : Entity
    {
        public delegate void AttackedLanded(AttackResult attack);

        public enum Type
        {
            Building,
            Ranger
        }

        [SerializeField] private List<Type> types = new List<Type>();

        public event System.Action<AgentObject> OnDestroyed;

        public override FactionType Faction => Agent.Faction;
        public int Direction { get; protected set; }
        public AgentEntity Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public int Priority { get => SpawnNumber; }
        public List<Type> Types { get => types; }
        public string Name => name;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyed?.Invoke(this);
        }

        public void Spawn(AgentEntity agent, int spawnNumber, int direction)
        {
            Direction = direction;
            SpawnNumber = spawnNumber;
            Agent = agent;

            transform.localScale = new Vector3(Mathf.Sign(direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public virtual void Activate() { }

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