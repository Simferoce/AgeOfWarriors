using Game.Agent;
using Game.Character;
using System.Globalization;
using System.Threading;
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

        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private bool cheatCost;
        [SerializeField] private int maxCharacter = 10;
        [SerializeField] private float technologyGainMultiplier = 1;

        public bool CheatCost { get => cheatCost; }
        public int MaxCharacter { get => maxCharacter; set => maxCharacter = value; }
        public float TechnologyGainMultiplier { get => technologyGainMultiplier; }

        private void Awake()
        {
            Instance = this;
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        [ContextMenu("LevelUp")]
        public void LevelUp()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            agentEntity.Technology.CurrentTechnology += agentEntity.Technology.MaxTechnology;
        }

        [ContextMenu("LevelToMaxLevel")]
        public void LevelToMaxLevel()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            agentEntity.Technology.CurrentTechnology += agentEntity.Technology.MaxTechnology * 20;
        }
    }
}