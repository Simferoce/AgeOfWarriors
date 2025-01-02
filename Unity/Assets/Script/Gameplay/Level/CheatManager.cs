using Game.Agent;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class CheatManager : Manager<CheatManager>
    {
        [SerializeField] private bool cheatCost;
        [SerializeField] private int maxCharacter = 10;
        [SerializeField] private float technologyGainMultiplier = 1;
        [SerializeField] private float factorySpeed = 1f;
        [SerializeField] private bool unrestrictedTechnology = false;

        public bool CheatCost { get => cheatCost; }
        public int MaxCharacter { get => maxCharacter; set => maxCharacter = value; }
        public float TechnologyGainMultiplier { get => technologyGainMultiplier; }
        public float FactorySpeed { get => factorySpeed; set => factorySpeed = value; }
        public bool UnrestrictedTechnology { get => unrestrictedTechnology; set => unrestrictedTechnology = value; }

        public override IEnumerator InitializeAsync()
        {
            yield break;
        }

        [ContextMenu("LevelUp")]
        public void LevelUp()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            agentEntity.Technology.CurrentTechnology += agentEntity.Technology.MaxTechnology;
        }

        [ContextMenu("LevelToMaxLevel")]
        public void LevelToMaxLevel()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            agentEntity.Technology.CurrentTechnology += agentEntity.Technology.MaxTechnology * 20;
        }
    }
}