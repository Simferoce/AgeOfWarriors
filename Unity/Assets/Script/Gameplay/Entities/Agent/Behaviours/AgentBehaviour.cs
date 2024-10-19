using System;

namespace Game.Agent
{
    [Serializable]
    public abstract class AgentBehaviour : IDisposable
    {
        protected AgentEntity agent;

        public virtual void Initialize(AgentEntity agent)
        {
            this.agent = agent;
            agent.Technology.OnLeveledUp += OnLevelUp;
        }

        public virtual void Update() { }

        public abstract void OnLevelUp();

        public void Dispose()
        {
            agent.Technology.OnLeveledUp -= OnLevelUp;
        }
    }
}
