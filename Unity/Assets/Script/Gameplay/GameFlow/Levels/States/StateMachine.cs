using UnityEngine;

namespace Game
{
    public partial class Level
    {
        public class StateMachine
        {
            public State Current { get; private set; }

            private State next = null;

            public void Initialize(State initialState)
            {
                initialState.Enter();
                Current = initialState;
            }

            public void Update()
            {
                if (next != null)
                {
                    Current.Exit();
                    Current = next;
                    Current.Enter();

                    next = null;
                }

                Current.Update();
            }

            public void End()
            {
                if (Current is not EndState)
                {
                    Debug.LogError("Expecting the level to be ended before it is exit.");
                    return;
                }

                Current.Exit();
            }

            public void ChangeState(State state)
            {
                if (next != null)
                {
                    Debug.Log($"Failed to change state to \"{state}\" as we are already changing state to \"{next}\"");
                    return;
                }

                next = state;
            }
        }
    }
}
