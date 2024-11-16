namespace AgeOfWarriors
{
    public partial class Character
    {
        public abstract class State
        {
            protected StateMachine stateMachine;

            public State(StateMachine stateMachine)
            {
                this.stateMachine = stateMachine;
            }

            public abstract void Enter();
            public abstract void Update();
            public abstract void Exit();
        }
    }
}
