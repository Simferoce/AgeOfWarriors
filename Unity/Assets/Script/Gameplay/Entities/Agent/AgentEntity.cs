using Game.Character;
using Game.Modifier;
using Game.Statistics;
using Game.Technology;
using System.Linq;
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

        public void SpawnAgentObject(CharacterDefinition characterDefinition, Vector3 position, int direction)
        {
            CharacterEntity character = characterDefinition.Spawn(this, position, nextSpawneeNumber++, direction);

            ModifierHandler modifierHandler = AddOrGetCachedComponent<ModifierHandler>();
            ModifierApplier modifierApplier = AddOrGetCachedComponent<ModifierApplier>();
            foreach (StatisticModifierBehaviour modifierBehaviour in modifierHandler.GetModifiers().SelectMany(x => x.Behaviours.OfType<StatisticModifierBehaviour>()))
            {
                if (modifierBehaviour.Definition is CharacterStatisticDefinition characterStatisticDefinition)
                {
                    ModifierHandler characterModifierHandler = character.AddOrGetCachedComponent<ModifierHandler>();
                    StatisticModifierBehaviour statisticModifierBehaviour = new StatisticModifierBehaviour()
                    {
                        Definition = characterStatisticDefinition.OutputDefinition,
                        Operator = modifierBehaviour.Operator,
                        Value = modifierBehaviour.Value.Snapshot()
                    };

                    modifierApplier.Apply(characterStatisticDefinition.ModifierDefinition, characterModifierHandler, new ModifierParameter<ModifierBehaviour>(null, statisticModifierBehaviour));
                }
            }

            OnAgentObjectSpawn?.Invoke(character.GetCachedComponent<AgentIdentity>());

            character.Initialize();
        }
    }
}
