using AgeOfWarriors.Core;
using System.Numerics;

namespace AgeOfWarriors
{
    public partial class Character : Entity
    {
        private ICharacterDefinition definition;
        private StateMachine stateMachine;

        public ICharacterDefinition Definition { get => definition; set => definition = value; }
        public float CurrentSpeed => stateMachine.Current is MoveState moveState ? moveState.CurrentSpeed : 0;
        public float Speed => definition.Speed;

        public Character(Agent agent, ICharacterDefinition definition)
            : base(agent.Game)
        {
            AddComponent(new Transform(agent.Game, Vector3.Zero, Quaternion.Identity));
            AddComponent(new AgentIdentity(agent));
            this.definition = definition;

            stateMachine = new StateMachine();
            stateMachine.Initialize(this);
        }

        public override void Update()
        {
            base.Update();
            stateMachine.Update();
        }
    }
}
