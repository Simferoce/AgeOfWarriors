using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierHandler))]
    public abstract class AgentObject : Entity
    {
        public delegate void AttackedLanded(AttackResult attack);

        public enum Type
        {
            Building
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            All = new List<AgentObject>();
        }

        public static List<AgentObject> All { get; private set; }

        [SerializeField] private List<Type> types = new List<Type>();

        public event System.Action<AgentObject> OnDestroyed;

        public override Faction Faction => Agent.Faction;
        public int Direction { get; protected set; }
        public Agent Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public int Priority { get => SpawnNumber; }
        public List<Type> Types { get => types; }
        public string Name => name;

        protected override void Awake()
        {
            base.Awake();
            All.Add(this);
        }

        protected virtual void OnDestroy()
        {
            All.Remove(this);
            OnDestroyed?.Invoke(this);
        }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

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