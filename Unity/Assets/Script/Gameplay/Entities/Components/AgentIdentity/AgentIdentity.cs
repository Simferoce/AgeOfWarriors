using Game.Statistics;
using UnityEngine;

namespace Game.Agent
{
    public class AgentIdentity : MonoBehaviour
    {
        public FactionType Faction => Agent.Faction;
        public int Direction { get; set; }
        public AgentEntity Agent { get; set; }
        public int SpawnNumber { get; set; }
        public int Priority { get => SpawnNumber; }
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

        public bool IsFirst()
        {
            int minPriority = int.MaxValue;
            foreach (var entity in Entity.All)
            {
                if (entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity characterIdentity)
                    && characterIdentity.Agent == Agent
                    && !(entity.StatisticRepository.TryGet("dead", out Statistic deadStatistic) || deadStatistic.Get<bool>()))
                {
                    int priority = characterIdentity.Priority;
                    if (priority < minPriority)
                    {
                        minPriority = priority;
                    }
                }
            }

            return minPriority == Priority;
        }
    }
}