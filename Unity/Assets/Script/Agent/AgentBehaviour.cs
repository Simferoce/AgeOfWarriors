using System;

namespace Game
{
    [Serializable]
    public abstract class AgentBehaviour
    {
        protected Agent agent;

        public virtual void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public abstract void Update();
    }
}
