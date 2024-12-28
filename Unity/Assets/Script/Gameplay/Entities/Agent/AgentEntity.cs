using Game.Character;
using Game.Modifier;
using Game.Statistics;
using Game.Technology;
using UnityEngine;

namespace Game.Agent
{
    [RequireComponent(typeof(ModifierApplier))]
    [RequireComponent(typeof(ModifierHandler))]
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

        public delegate void AgentObjectSpawnDelegate(AgentIdentity agentIdentity);

        public event AgentObjectSpawnDelegate OnAgentObjectSpawn;

        public AgentFactory Factory { get => factory; set => factory = value; }
        public BaseEntity Base { get => agentBase; set => agentBase = value; }
        public float Currency { get; set; }
        public int Direction { get => direction; set => direction = value; }
        public TechnologyHandler Technology { get => technology; }
        public AgentLoadout Loadout { get => loadout; set => loadout = value; }
        public FactionType Faction { get => faction; set => faction = value; }

        private int nextSpawneeNumber = 0;

        protected override void Awake()
        {
            base.Awake();
            factory.Initialize(this);
            loadout.Initialize(this);
            technology.Initialize(this);
            agentBehaviour.Initialize(this);

            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => Faction));
            base.Initialize();
        }

        private void Start()
        {
            AgentIdentity agentIdentity = agentBase.AddOrGetCachedComponent<AgentIdentity>();
            agentIdentity.Set(this, nextSpawneeNumber++, direction);
            agentBase.Initialize();
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
            agentBehaviour.Dispose();
        }

        public bool TryQueueSpawnAgentObject(int index)
        {
            CharacterDefinition characterDefinition = Loadout.GetCharacterDefinitionAtIndex(index);
            return factory.QueueLaneObject(new AgentFactoryCommand(this, agentBase.SpawnPoint, characterDefinition.ProductionDuration / LevelSetup.Instance.FactorySpeed, characterDefinition), characterDefinition.Cost);
        }

        public CharacterEntity SpawnAgentObject(CharacterDefinition characterDefinition, Vector3 position, int direction)
        {
            return SpawnAgentObject(characterDefinition, position, direction, nextSpawneeNumber++);
        }

        public CharacterEntity SpawnAgentObject(CharacterDefinition characterDefinition, Vector3 position, int direction, int spawnNumber)
        {
            CharacterEntity character = characterDefinition.Spawn(this, position, spawnNumber, direction);
            OnAgentObjectSpawn?.Invoke(character.GetCachedComponent<AgentIdentity>());
            character.Initialize();

            character.name = $"{characterDefinition.Title} - {faction}";
            return character;
        }
    }
}
