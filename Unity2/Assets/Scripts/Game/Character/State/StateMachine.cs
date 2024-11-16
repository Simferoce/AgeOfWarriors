namespace AgeOfWarriors
{
    public partial class Character
    {
        public class StateMachine
        {
            private State current;
            private Character character;

            public Character Character { get => character; }
            public State Current { get => current; }

            public void Initialize(Character character)
            {
                this.character = character;
                current = new MoveState(this);
                current.Enter();
            }

            public void Update()
            {
                current.Update();
            }
        }
    }
}
