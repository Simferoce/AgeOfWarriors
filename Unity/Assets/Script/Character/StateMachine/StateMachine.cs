namespace Game
{
    public partial class Character
    {
        public class StateMachine
        {
            public State Current { get; private set; }
            public State Next { get; private set; }

            public void Initialize(State initial)
            {
                Current = initial;
                Current.Enter();
            }

            public void Update()
            {
                if (Next != null)
                {
                    Current.Exit();
                    Current = Next;
                    Current.Enter();

                    Next = null;
                }

                if (Current != null)
                    Current.Update();
            }

            public void SetState(State state)
            {
                Next = state;
            }
        }
    }
}
