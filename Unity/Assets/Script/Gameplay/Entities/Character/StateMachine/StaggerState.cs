using Game.Components;

namespace Game.Character
{
    public partial class CharacterEntity
    {
        public class StaggerState : State
        {
            public StaggerState(CharacterEntity character) : base(character)
            {
            }

            protected override void InternalEnter()
            {
                character.Animated.Play("Stagger");
                character.GetCachedComponent<Caster>().Interupt();
            }

            protected override void InternalExit()
            {
                character.Animated.Play("Move");
            }

            protected override void InternalUpdate()
            {
                character.rigidbody.linearVelocityX = 0;
                if (!character.IsStaggered)
                {
                    character.stateMachine.SetState(new MoveState(character));
                }
            }
        }
    }
}
