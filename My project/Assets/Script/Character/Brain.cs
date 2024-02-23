using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private Character character;

    public float SpeedRatio { get; private set; }

    public void Initialize(Character character)
    {
        this.character = character;
    }

    public void Tick()
    {
        List<Target> intersectedObjects = character.Lane.Intersecting<Target>(character.Min, character.Max);
        Target intersecting = intersectedObjects.Where(x => x != character).FirstOrDefault();

        SpeedRatio = intersecting == null ? 1 : 0;

        if(intersecting != null && character.StateMachine.Current is not CharacterAttackState)
        {
            character.StateMachine.SetState(new CharacterAttackState(character));
        }
        else if(intersecting == null && character.StateMachine.Current is not CharacterMoveState)
        {
            character.StateMachine.SetState(new CharacterMoveState(character));
        }
    }
}

