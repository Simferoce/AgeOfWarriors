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
                character.Animated.ClearTrigger("EndStagger");
                character.Animated.SetTrigger("Stagger");
                character.Entity.GetCachedComponent<Caster>().Interupt();
            }

            protected override void InternalExit()
            {
                character.Animated.ClearTrigger("Stagger");
            }

            protected override void InternalUpdate()
            {
                if (!character.IsStaggered)
                {
                    character.stateMachine.SetState(new MoveState(character));
                    character.Animated.SetTrigger("EndStagger");
                }
            }
        }
    }
}
