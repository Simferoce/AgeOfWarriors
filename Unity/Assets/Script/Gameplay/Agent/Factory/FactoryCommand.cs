using UnityEngine;

namespace Game
{
    public class FactoryCommand
    {
        public SpawnPoint SpawnPoint { get; set; }
        public float ProductionDuration { get; set; }
        public AgentObjectDefinition AgentObjectDefinition { get; set; }
        public Agent Agent { get; set; }
        public float Progress => Mathf.Clamp01((startedAt + ProductionDuration - Time.time) / ProductionDuration);
        private float startedAt;

        public FactoryCommand(Agent agent, SpawnPoint spawnPoint, float productionDuration, AgentObjectDefinition agentObjectDefinition)
        {
            this.Agent = agent;
            SpawnPoint = spawnPoint;
            ProductionDuration = productionDuration;
            AgentObjectDefinition = agentObjectDefinition;
        }

        public void Execute(Factory factory)
        {
            Agent.SpawnAgentObject(AgentObjectDefinition, SpawnPoint.transform.position, SpawnPoint.Direction);
        }

        public void Start()
        {
            startedAt = Time.time;
        }

        public bool IsFinish()
        {
            return startedAt + ProductionDuration < Time.time;
        }
    }
}
