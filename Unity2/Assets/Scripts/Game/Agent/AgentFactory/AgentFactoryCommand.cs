using AgeOfWarriors;

namespace AgeOfWarriors
{
    public class AgentFactoryCommand
    {
        private CharacterFactory characterFactory;
        private float duration;
        private float startedAt;
        private Agent agent;
        private SpawnPoint spawnPoint;

        public AgentFactoryCommand(Agent agent, SpawnPoint spawnPoint, CharacterFactory characterFactory, float duration)
        {
            this.agent = agent;
            this.spawnPoint = spawnPoint;
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
            Character character = characterFactory.Instantiate(agent, spawnPoint.GetComponent<Transform>().Position, spawnPoint.GetComponent<Transform>().Rotation);
            character.Initialize();

            agent.AssignCharacter(character);
        }
    }
}
