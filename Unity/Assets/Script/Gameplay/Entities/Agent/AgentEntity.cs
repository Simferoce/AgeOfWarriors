using Game.Modifier;
using Game.Technology;
using UnityEngine;

namespace Game.Agent
{
    [RequireComponent(typeof(ModifierApplier))]
    public class AgentEntity : Entity
    {
        [SerializeField] private FactionType faction;
        [SerializeField] private BaseEntity agentBase;
        [SerializeField] private int direction;
        [SerializeField] private float currencyGainRate = 5f;
        [SerializeField] private TechnologyHandler technology;
        [SerializeField] private AgentFactory factory;
        [SerializeField] private AgentLoadout loadout;
        [SerializeReference, SubclassSelector] private AgentBehaviour agentBehaviour;

        public delegate void AgentObjectSpawnDelegate(AgentObject agentObject);

        public event AgentObjectSpawnDelegate OnAgentObjectSpawn;

        public override FactionType Faction => faction;
        public AgentFactory Factory { get => factory; set => factory = value; }
        public BaseEntity Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public int Direction { get => direction; set => direction = value; }
        public TechnologyHandler Technology { get => technology; }
        public AgentLoadout Loadout { get => loadout; set => loadout = value; }

        private int nextSpawneeNumber = 0;

        protected override void Awake()
        {
            AgentRepository.Instance.Add(this);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AgentRepository.Instance.Remove(this);
            agentBehaviour.Dispose();
        }

        public bool TryQueueSpawnAgentObject(int index)
        {
            AgentObjectDefinition agentObjectDefinition = Loadout.GetAgentObjectDefinitionAtIndex(index);
            return factory.QueueLaneObject(new AgentFactoryCommand(this, agentBase.SpawnPoint, agentObjectDefinition.ProductionDuration, agentObjectDefinition), agentObjectDefinition.Cost);
        }

        public void SpawnAgentObject(AgentObjectDefinition agentObjectDefinition, Vector3 position, int direction)
        {
            AgentObject agentObject = agentObjectDefinition.Spawn(this, position, nextSpawneeNumber++, direction);
            OnAgentObjectSpawn?.Invoke(agentObject);
        }
    }
}
