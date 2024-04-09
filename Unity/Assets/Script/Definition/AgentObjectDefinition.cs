using UnityEngine;

namespace Game
{
    public abstract class AgentObjectDefinition : Definition
    {
        [Header("Display")]
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;

        [Header("General - Statistics")]
        [SerializeField] private float productionDuration;
        [SerializeField] private float cost;
        [SerializeField] private float technologyGainPerSecond;

        [SerializeField] private StatisticHolder statistics;
        public StatisticHolder Statistics => statistics;

        public Sprite Icon { get => icon; }
        public float ProductionDuration { get => productionDuration; set => productionDuration = value; }
        public float Cost { get => cost; set => cost = value; }
        public float TechnologyGainPerSecond { get => technologyGainPerSecond; set => technologyGainPerSecond = value; }
        public string Title { get => title; set => title = value; }

        public virtual bool IsSpecialization(AgentObjectDefinition agentObjectDefinition) { return false; }
        public abstract AgentObject Spawn(Agent agent, Vector3 position, int spawnNumber, int direction);
    }
}