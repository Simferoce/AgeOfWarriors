using Game.Agent;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class LevelSetup : MonoBehaviour
    {
        public static LevelSetup Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Instance = null;
        }

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

        private void Awake()
        {
            Instance = this;
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