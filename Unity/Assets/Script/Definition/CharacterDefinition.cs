using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/AgentObject/Character")]
    public class CharacterDefinition : AgentObjectDefinition
    {
        [SerializeField] private GameObject prefab;

        public override AgentObject Spawn(Agent agent, Vector3 position, int spawnNumber, int direction)
        {
            Character character = GameObject.Instantiate(prefab, position, Quaternion.identity).GetComponent<Character>();
            character.Definition = this;
            character.Spawn(agent, spawnNumber, direction);

            return character;
        }
    }
}