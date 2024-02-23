using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CharacterAttackState : CharacterState
{
    public CharacterAttackState(Character character) : base(character)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.CharacterAnimator.SetTrigger(CharacterAnimator.ATTACK);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
