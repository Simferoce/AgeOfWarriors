namespace Game.Character
{
    public partial class CharacterEntity
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

                    //Debug.Log($"{Current.Character.name}:{Current.Character.GetInstanceID()} Switch State: {Current.GetType().Name}");
                    Next = null;
                }

                if (Current != null)
                    Current.Update();
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
