namespace AgeOfWarriors
{
    public abstract class AgentBehaviour
    {
        protected Agent agent;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public virtual void Update()
        {
        }
    }
}
