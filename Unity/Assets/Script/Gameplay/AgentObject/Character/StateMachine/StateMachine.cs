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

            public void FixedUpdate()
            {
                if (Next != null)
                {
                    Current.Exit();
                    Current = Next;
                    Current.Enter();

                    //Debug.Log($"{Current.Character.name}:{Current.Character.GetInstanceID()} Switch State: {Current.GetType().Name}");
                    Next = null;
                }

                if (Current != null)
                    Current.FixedUpdate();
            }

            public void SetState(State state)
            {
                if (Current.CanExit)
                    Next = state;

                //Debug.Log($"{state.Character.name}:{state.Character.GetInstanceID()} Set Next State: {state.GetType().Name}");
            }
        }
    }
}
