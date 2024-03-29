using System;

namespace Game
{
    [Serializable]
    public abstract class AgentBehaviour : IDisposable
    {
        protected Agent agent;

        public virtual void Initialize(Agent agent)
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
