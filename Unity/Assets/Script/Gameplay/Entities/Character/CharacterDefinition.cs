using Game.Agent;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Character
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/AgentObject/Character")]
    public class CharacterDefinition : Definition
    {
        [Header("Display")]
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;

        [Header("Base")]
        [SerializeField] private CharacterDefinition baseDefinition;

        [Header("Character - Statistic")]
        [SerializeField] private float productionDuration;
        [SerializeField] private float cost;
        [SerializeField] private float reach = 1f;
        [SerializeField] private float speed = 1f;
        [SerializeField, FormerlySerializedAs("attackPerSeconds")] private float attackSpeed = 1f;
        [SerializeField] private float attackPower = 1f;
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float defense = 5f;
        [SerializeField] private float technologyGainPerSecond;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab;

        public Sprite Icon { get => icon; }
        public string Title { get => title; set => title = value; }

        public float Reach => reach;
        public float Speed => speed;
        public float AttackSpeed => attackSpeed;
        public float AttackPower => attackPower;
        public float MaxHealth => maxHealth;
        public float Defense => defense;
        public float TechnologyGainPerSecond => technologyGainPerSecond;
        public float ProductionDuration { get => productionDuration; set => productionDuration = value; }
        public float Cost { get => cost; set => cost = value; }

        public bool IsSpecialization(CharacterDefinition agentIdentityDefinition)
        {
            return baseDefinition == agentIdentityDefinition || (baseDefinition?.IsSpecialization(agentIdentityDefinition) ?? false);
        }

        public CharacterEntity Spawn(AgentEntity agent, Vector3 position, int spawnNumber, int direction)
        {
            CharacterEntity character = GameObject.Instantiate(prefab, position, Quaternion.identity).GetComponent<CharacterEntity>();
            character.Definition = this;

            AgentIdentity agentIdentity = character.AddOrGetCachedComponent<AgentIdentity>();
            agentIdentity.Set(agent, spawnNumber, direction);

            return character;
        }
    }
}