using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/AgentObject/Character")]
    public class CharacterDefinition : AgentObjectDefinition
    {
        [Header("Base")]
        [SerializeField] private CharacterDefinition baseDefinition;

        [Header("Character - Statistic")]
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat reach = new StatisticSerializeFloat(0.5f);
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat speed = new StatisticSerializeFloat(1f);
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat attackPerSeconds = new StatisticSerializeFloat(1f);
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat attackPower = new StatisticSerializeFloat(1f);
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat maxHealth = new StatisticSerializeFloat(10f);
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat defense = new StatisticSerializeFloat(5f);

        [Header("Prefab")]
        [SerializeField] private GameObject prefab;

        public IStatisticFloat Reach { get => reach; }
        public IStatisticFloat Speed { get => speed; }
        public IStatisticFloat AttackPerSeconds { get => attackPerSeconds; }
        public IStatisticFloat AttackPower { get => attackPower; }
        public IStatisticFloat MaxHealth { get => maxHealth; }
        public IStatisticFloat Defense { get => defense; }

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