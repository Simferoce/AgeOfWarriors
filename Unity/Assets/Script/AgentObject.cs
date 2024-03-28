using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class AgentObject : MonoBehaviour
    {
        public static List<AgentObject> AgentObjects { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            AgentObjects = new List<AgentObject>();
        }

        public int Direction { get; protected set; }
        public Agent Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public virtual float TechnologyGainPerSecond { get => 0f; }

        private void Awake()
        {
            AgentObjects.Add(this);
        }

        private void OnDestroy()
        {
            AgentObjects.Remove(this);
        }

        public virtual AgentObjectDefinition GetDefinition() { return null; }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            if (direction < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public abstract class AgentObject<T> : AgentObject
        where T : AgentObjectDefinition
    {
        public override float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public T Definition { get; set; }

        public override AgentObjectDefinition GetDefinition()
        {
            return Definition;
        }
    }
}