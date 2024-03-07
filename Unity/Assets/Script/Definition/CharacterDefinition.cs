using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/LaneObject/Character")]
    public class CharacterDefinition : LaneObjectDefinition
    {
        [SerializeField] private GameObject prefab;

        public override LaneObject Spawn(Agent agent, int spawnNumber, float position, int direction)
        {
            Character character = GameObject.Instantiate(prefab).GetComponent<Character>();
            character.Spawn(agent, spawnNumber, position, direction);

            return character;
        }
    }
}