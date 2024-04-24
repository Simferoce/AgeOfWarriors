namespace Game
{
    public partial class Character
    {
        public class CastingState : State
        {
            public CastingState(Character character) : base(character)
            {
            }

            protected override void InternalEnter()
            {

            }

            protected override void InternalExit()
            {

            }

            protected override void InternalUpdate()
            {
                CheckStagger();
            }
        }
    }
}
