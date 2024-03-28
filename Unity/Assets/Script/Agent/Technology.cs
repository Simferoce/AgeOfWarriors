using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Technology
    {
        [SerializeField] private float maxTechnology;

        public float CurrentTechnology { get; set; }
        public float CurrentLevel { get; set; }
        public float CurrentTechnologyNormalized { get => CurrentTechnology / maxTechnology; }

        public event Action OnLevelUp;

        private Agent agent;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public void Update()
        {
            foreach (AgentObject agentObject in AgentObject.AgentObjects)
            {
                if (agentObject.Agent == agent)
                {
                    CurrentTechnology += agentObject.TechnologyGainPerSecond * Time.deltaTime;
                }
            }

            if (CurrentTechnology > maxTechnology)
            {
                CurrentTechnology -= maxTechnology;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            CurrentLevel++;
        }
    }
}
