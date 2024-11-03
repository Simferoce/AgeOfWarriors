using Game.Character;
using UnityEngine;

namespace Game.Agent
{
    public class AgentFactoryCommand
    {
        public SpawnPoint SpawnPoint { get; set; }
        public float ProductionDuration { get; set; }
        public CharacterDefinition CharacterDefinition { get; set; }
        public AgentEntity Agent { get; set; }
        public float Progress => Mathf.Clamp01((startedAt + ProductionDuration - Time.time) / ProductionDuration);
        private float startedAt;

        public AgentFactoryCommand(AgentEntity agent, SpawnPoint spawnPoint, float productionDuration, CharacterDefinition characterDefinition)
        {
            this.Agent = agent;
            SpawnPoint = spawnPoint;
            ProductionDuration = productionDuration;
            CharacterDefinition = characterDefinition;
        }

        public void Execute(AgentFactory factory)
        {
            Agent.SpawnAgentObject(CharacterDefinition, SpawnPoint.transform.position, SpawnPoint.Direction);
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
