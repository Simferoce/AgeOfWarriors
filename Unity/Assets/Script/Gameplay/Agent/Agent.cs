using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierApplier))]
    public class Agent : Entity
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
        [SerializeField] private TechnologyHandler technology;
        [SerializeField] private Factory factory;
        [SerializeReference, SubclassSelector] private AgentBehaviour agentBehaviour;

        public override Faction Faction => faction;
        public Factory Factory { get => factory; set => factory = value; }
        public Base Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public int Direction { get => direction; set => direction = value; }
        public TechnologyHandler Technology { get => technology; }

        protected override void Awake()
        {
            base.Awake();
            factory.Initialize(this);
            technology.Initialize(this);
            agentBehaviour.Initialize(this);
        }

        private void Start()
        {
            agentBase.Spawn(this, 0, direction);
        }

        private void Update()
        {
            factory.Update();
            technology.Update();
            agentBehaviour.Update();

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

        private void OnDestroy()
        {
            agentBehaviour.Dispose();
        }

        public bool SpawnLaneObject(int index)
        {
            AgentObjectDefinition agentObjectDefinition = Factory.GetAgentObjectDefinitionAtIndex(index);
            return factory.QueueLaneObject(agentBase.SpawnPoint, agentObjectDefinition);
        }
    }
}
