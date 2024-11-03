using UnityEngine;

namespace Game.Agent
{
    public class AgentIdentity : MonoBehaviour
    {
        public FactionType Faction => Agent.Faction;
        public int Direction { get; protected set; }
        public AgentEntity Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public int Priority { get => SpawnNumber; }
        public string Name => name;
        public Entity Entity { get; set; }

        public void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public void Set(AgentEntity agent, int spawnNumber, int direction)
        {
            Direction = direction;
            SpawnNumber = spawnNumber;
            Agent = agent;

            transform.localScale = new Vector3(Mathf.Sign(direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}