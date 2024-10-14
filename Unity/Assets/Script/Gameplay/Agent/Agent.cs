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
        [SerializeField] private AgentLoadout loadout;
        [SerializeReference, SubclassSelector] private AgentBehaviour agentBehaviour;

        public delegate void AgentObjectSpawnDelegate(AgentObject agentObject);

        public event AgentObjectSpawnDelegate OnAgentObjectSpawn;

        public override Faction Faction => faction;
        public Factory Factory { get => factory; set => factory = value; }
        public Base Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public int Direction { get => direction; set => direction = value; }
        public TechnologyHandler Technology { get => technology; }
        public AgentLoadout Loadout { get => loadout; set => loadout = value; }

        private int nextSpawneeNumber = 0;

        protected override void Awake()
        {
            base.Awake();
            factory.Initialize(this);
            loadout.Initialize(this);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            agentBehaviour.Dispose();
        }

        public bool TryQueueSpawnAgentObject(int index)
        {
            AgentObjectDefinition agentObjectDefinition = Loadout.GetAgentObjectDefinitionAtIndex(index);
            return factory.QueueLaneObject(new FactoryCommand(this, agentBase.SpawnPoint, agentObjectDefinition.ProductionDuration, agentObjectDefinition), agentObjectDefinition.Cost);
        }

        public void SpawnAgentObject(AgentObjectDefinition agentObjectDefinition, Vector3 position, int direction)
        {
            AgentObject agentObject = agentObjectDefinition.Spawn(this, position, nextSpawneeNumber++, direction);
            OnAgentObjectSpawn?.Invoke(agentObject);
        }
    }
}
