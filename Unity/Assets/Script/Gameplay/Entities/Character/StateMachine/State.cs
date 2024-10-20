using UnityEngine;

namespace Game.Character
{
    public partial class CharacterEntity
    {
        public abstract class State
        {
            public virtual bool CanExit => true;

            public CharacterEntity Character { get => character; }

            protected CharacterEntity character;
            protected float enteredAt;

            protected State(CharacterEntity character)
            {
                this.character = character;
            }

            public void Enter()
            {
                enteredAt = Time.time;
                InternalEnter();
            }

            public void Update()
            {
                InternalUpdate();
            }

            public void Exit()
            {
                InternalExit();
            }

            protected abstract void InternalEnter();
            protected abstract void InternalUpdate();
            protected abstract void InternalExit();

            public void CheckStagger()
            {
                if (character.IsStaggered)
                    character.stateMachine.SetState(new StaggerState(character));
            }
        }
    }
}
