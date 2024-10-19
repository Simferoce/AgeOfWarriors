namespace Game.Character
{
    public partial class CharacterEntity
    {
        public class CastingState : State
        {
            public CastingState(CharacterEntity character) : base(character)
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
