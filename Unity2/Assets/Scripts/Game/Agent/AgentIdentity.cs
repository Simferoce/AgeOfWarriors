using AgeOfWarriors.Core;

namespace AgeOfWarriors
{
    public class AgentIdentity : Component
    {
        private Agent agent;

        public AgentIdentity(Agent agent)
            : base(agent.Game)
        {
            this.agent = agent;
        }
    }
}
