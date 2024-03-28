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
        [SerializeField] private float currencyGainRate = 5f;
        [SerializeField] private Technology technology;
        [SerializeField] private Factory factory;
        [SerializeReference, SubclassSelector] private AI ai;

        public Faction Faction { get => faction; }
        public Factory Factory { get => factory; set => factory = value; }
        public Base Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public int Direction { get => direction; set => direction = value; }
        public Technology Technology { get => technology; }

        private void Awake()
        {
            factory.Initialize(this);
            technology.Initialize(this);

            if (ai != null)
                ai.Initialize(this);
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

            if (ai != null)
                ai.Update();

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

        public bool SpawnLaneObject(int index)
        {
            AgentObjectDefinition agentObjectDefinition = Factory.GetAgentObjectDefinitionAtIndex(index);
            return factory.QueueLaneObject(agentBase.SpawnPoint, agentObjectDefinition);
        }
    }
}
