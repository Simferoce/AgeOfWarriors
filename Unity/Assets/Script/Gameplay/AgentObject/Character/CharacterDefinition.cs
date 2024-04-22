using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/AgentObject/Character")]
    public class CharacterDefinition : AgentObjectDefinition
    {
        [Header("Base")]
        [SerializeField] private CharacterDefinition baseDefinition;

        [Header("Character - Statistic")]
        [SerializeField] private float reach = 1f;
        [SerializeField] private float speed = 1f;
        [SerializeField, FormerlySerializedAs("attackPerSeconds")] private float attackSpeed = 1f;
        [SerializeField] private float attackPower = 1f;
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float defense = 5f;
        [SerializeField] private float technologyGainPerSecond;

        [Header("Prefab")]
        [SerializeField] private GameObject prefab;

        [Statistic("reach")] public float Reach(Character character) => reach;
        [Statistic("speed")] public float Speed(Character character) => speed;
        [Statistic("attack_speed")] public float AttackSpeed(Character character) => attackSpeed;
        [Statistic("attack_power")] public float AttackPower(Character character) => attackPower;
        [Statistic("max_health")] public float MaxHealth(Character character) => maxHealth;
        [Statistic("defense")] public float Defense(Character character) => defense;
        [Statistic("technology")] public float TechnologyGainPerSecond(Character character) => technologyGainPerSecond;

        public override bool IsSpecialization(AgentObjectDefinition agentObjectDefinition)
        {
            return baseDefinition == agentObjectDefinition || (baseDefinition?.IsSpecialization(agentObjectDefinition) ?? false);
        }

        public override AgentObject Spawn(Agent agent, Vector3 position, int spawnNumber, int direction)
        {
            Character character = GameObject.Instantiate(prefab, position, Quaternion.identity).GetComponent<Character>();
            character.Definition = this;
            character.Spawn(agent, spawnNumber, direction);

            return character;
        }
    }
}