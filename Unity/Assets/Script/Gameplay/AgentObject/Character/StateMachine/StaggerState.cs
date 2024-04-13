using UnityEngine;

namespace Game
{
    public partial class Character
    {
        public class StaggerState : State
        {
            private float duration;

            public StaggerState(Character character, float duration) : base(character)
            {
                this.duration = duration;
            }

            protected override void InternalEnter()
            {
                character.CharacterAnimator.SetTrigger(CharacterAnimatorParameter.Parameter.Stagger);
            }

            protected override void InternalExit()
            {
                character.CharacterAnimator.SetTrigger(CharacterAnimatorParameter.Parameter.EndStagger);
            }

            protected override void InternalUpdate()
            {
                if (Time.time - enteredAt > duration)
                    character.stateMachine.SetState(new MoveState(character));
            }
        }
    }
}
