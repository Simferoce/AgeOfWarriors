using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMoveState : CharacterState
{
    public CharacterMoveState(Character character) : base(character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        character.CharacterAnimator.SetTrigger(CharacterAnimator.MOVE);
    }

    public override void Update()
    {
        base.Update();
        character.CharacterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, character.Brain.SpeedRatio);

        LaneMovementResult laneMovementResult = character.Lane.CheckMovement(character.Position, character.Speed * character.Brain.SpeedRatio * Time.deltaTime);
        character.Position = laneMovementResult.NewPosition;
    }
}

