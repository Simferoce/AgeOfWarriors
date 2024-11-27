using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Character
    {
        public abstract class State
        {
            public virtual bool CanExit => true;

            public Character Character { get => character; }

            protected Character character;
            protected float enteredAt;

            protected State(Character character)
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
                if (character.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsStagger != null).Any(x => x.IsStagger.Value))
                {
                    character.stateMachine.SetState(new StaggerState(character));
                }
            }
        }
    }
}
