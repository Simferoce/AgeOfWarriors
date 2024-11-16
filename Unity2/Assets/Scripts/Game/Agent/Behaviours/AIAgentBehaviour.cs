namespace AgeOfWarriors
{
    public class AIAgentBehaviour : AgentBehaviour
    {
        private float delayBetweenSpawn = 1f;
        private float lastSpawn = 0.0f;

        public override void Update()
        {
            base.Update();

            if (agent.Game.Time.CurrentTime > lastSpawn + delayBetweenSpawn)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            agent.Factory.TryEnqueue(new AgentFactoryCommand(agent, agent.Loadout.GetCharacterFactoryAtIndex(0), 0.5f));
            lastSpawn = agent.Game.Time.CurrentTime;
        }
    }
}
