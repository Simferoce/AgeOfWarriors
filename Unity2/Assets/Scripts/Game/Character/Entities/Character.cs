using AgeOfWarriors.Core;
using System.Numerics;

namespace AgeOfWarriors
{
    public class Character : Entity
    {
        private ICharacterDefinition definition;
        public ICharacterDefinition Definition { get => definition; set => definition = value; }

        public Character(Agent agent, ICharacterDefinition definition)
            : base(agent.Game)
        {
            AddComponent(new Transform(agent.Game, Vector3.Zero, Quaternion.Identity));
            AddComponent(new AgentIdentity(agent));
            this.definition = definition;
        }
    }
}
