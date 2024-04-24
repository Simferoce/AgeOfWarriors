using System.Linq;

namespace Game
{
    public partial class Character
    {
        public class StaggerState : State
        {
            public StaggerState(Character character) : base(character)
            {
            }

            protected override void InternalEnter()
            {
                character.CharacterAnimator.ClearTrigger(CharacterAnimatorParameter.Parameter.EndStagger);
                character.CharacterAnimator.SetTrigger(CharacterAnimatorParameter.Parameter.Stagger);

                foreach (Ability ability in character.abilities)
                {
                    if (ability.IsActive)
                        ability.Interrupt();
                }
            }

            protected override void InternalExit()
            {
                character.CharacterAnimator.ClearTrigger(CharacterAnimatorParameter.Parameter.Stagger);
            }

            protected override void InternalUpdate()
            {
                if (!character.GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.IsStagger != null).Any(x => x.IsStagger.Value))
                {
                    character.stateMachine.SetState(new MoveState(character));
                    character.CharacterAnimator.SetTrigger(CharacterAnimatorParameter.Parameter.EndStagger);
                }
            }
        }
    }
}
