using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Agent : MonoBehaviour
    {
        public static List<Agent> agents = new List<Agent>();
        public static Agent Player => agents.FirstOrDefault(x => x.Faction == Faction.Player);
        public static Agent Opponent => agents.FirstOrDefault(x => x.Faction == Faction.Opponent);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            agents = new List<Agent>();
        }

        [SerializeField] private Faction faction;
        [SerializeField] private Base agentBase;
        [SerializeField] private int direction;
        [SerializeField] private bool infiniteCurrency = false;
        [SerializeField] private float currencyGainRate = 5f;
        [SerializeField] private Technology technology;

        private Factory factory;

        public Faction Faction { get => faction; }
        public Factory Factory { get => factory; set => factory = value; }
        public Base Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public bool InfiniteMoney { get => infiniteCurrency; set => infiniteCurrency = value; }
        public int Direction { get => direction; set => direction = value; }
        public Technology Technology { get => technology; }

        private void Awake()
        {
            factory = new Factory(this);
            technology.Initialize(this);
        }

        private void Start()
        {
            agentBase.transform.position = Lane.Instance.Project(agentBase.transform.position);
            agentBase.Spawn(this, 0, direction);
        }

        private void Update()
        {
            factory.Update();
            technology.Update();

            Currency += currencyGainRate * Time.deltaTime;
        }

        private void OnEnable()
        {
            agents.Add(this);
        }

        private void OnDisable()
        {
            agents.Remove(this);
        }

        public void SpawnLaneObject(AgentObjectDefinition laneObjectDefinition)
        {
            factory.QueueLaneObject(agentBase.SpawnPoint, laneObjectDefinition);
        }
    }
}
