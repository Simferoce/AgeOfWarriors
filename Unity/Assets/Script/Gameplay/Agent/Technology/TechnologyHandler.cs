using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TechnologyHandler
    {
        [SerializeField] private float maxTechnology;

        public float CurrentTechnology { get; set; }
        public float CurrentLevel { get; set; }
        public float CurrentTechnologyNormalized { get => CurrentTechnology / maxTechnology; }
        public List<TechnologyPerkDefinition> PerksUnlocked { get => perksUnlocked; }
        public float MaxTechnology { get => maxTechnology; set => maxTechnology = value; }

        private Agent agent;
        private List<TechnologyPerkDefinition> perksUnlocked = new List<TechnologyPerkDefinition>();

        public event Action OnLeveledUp;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public void Update()
        {
            foreach (AgentObject agentObject in AgentObject.All)
            {
                if (agentObject is Character character && agentObject.Agent == agent)
                {
                    CurrentTechnology += Level.Instance.TechnologyGainMultiplier * character.TechnologyGainPerSecond * Time.deltaTime;
                }
            }

            if (CurrentTechnology >= maxTechnology)
            {
                CurrentTechnology -= maxTechnology;
                LevelUp();
            }
        }

        public void Acquire(TechnologyPerkDefinition definition)
        {
            PerksUnlocked.Add(definition);
        }

        private void LevelUp()
        {
            CurrentLevel++;
            OnLeveledUp?.Invoke();
        }
    }
}
