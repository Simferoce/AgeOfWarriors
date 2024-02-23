using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Definition/LaneObject/Character")]
public class CharacterDefinition : LaneObjectDefinition
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;

    public override LaneObject Instantiate(Lane lane, float position, int direction)
    {
        Character character = GameObject.Instantiate(prefab).GetComponent<Character>();
        Debug.Assert(character != null);

        character.Spawn(lane, position, direction, speed);

        return character;
    }
}
