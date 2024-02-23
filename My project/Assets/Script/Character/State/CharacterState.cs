using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CharacterState : State
{
    protected Character character;

    public CharacterState(Character character) : base(character.StateMachine)
    {
        this.character = character;
    }
}

