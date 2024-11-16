using AgeOfWarriors.Core;
using System.Numerics;

namespace AgeOfWarriors
{
    public partial class Character
    {
        public class MoveState : State
        {
            public float CurrentSpeed { get; private set; }

            public MoveState(StateMachine stateMachine) : base(stateMachine)
            {
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
            }

            public override void Update()
            {
                Transform transform = stateMachine.Character.GetComponent<Transform>();
                CurrentSpeed = stateMachine.Character.Speed;

                transform.Translate(CurrentSpeed * stateMachine.Character.Game.Time.DeltaTime * new Vector3(1, 0, 0));
            }
        }
    }
}
