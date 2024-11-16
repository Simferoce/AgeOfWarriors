namespace AgeOfWarriors
{
    public class AgentFactoryCommand
    {
        private CharacterFactory characterFactory;
        private float duration;
        private float startedAt;
        private Agent agent;

        public AgentFactoryCommand(Agent agent, CharacterFactory characterFactory, float duration)
        {
            this.agent = agent;
            this.characterFactory = characterFactory;
            this.duration = duration;
        }

        public void Start()
        {
            startedAt = agent.Game.Time.CurrentTime;
        }

        public bool IsFinish()
        {
            return agent.Game.Time.CurrentTime > startedAt + duration;
        }

        public void Execute()
        {
            Character character = characterFactory.Instantiate(agent);
            character.Initialize();
            agent.AssignCharacter(character);
        }
    }
}
