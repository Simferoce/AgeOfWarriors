using AgeOfWarriors.Core;
using System.Collections.Generic;

namespace AgeOfWarriors
{
    public class Game
    {
        public Time Time { get => time; }
        public List<Entity> Entities { get => entities; set => entities = value; }
        public IGameDebug Debug { get => gameDebug; }
        public IDefinitionRepository DefinitionRepository { get => definitionRepository; }
        public EventChannel EventChannel { get => eventChannel; }

        private Time time = new Time();
        private List<Agent> agents = new List<Agent>();
        private List<Entity> entities = new List<Entity>();
        private EventChannel eventChannel = new EventChannel();
        private IGameDebug gameDebug;
        private IDefinitionRepository definitionRepository;

        public void Initialize(IGameDebug gameDebug, IDefinitionRepository definitionRepository)
        {
            this.gameDebug = gameDebug;
            this.definitionRepository = definitionRepository;

            AIAgentBehaviour agentBehaviour = new AIAgentBehaviour();
            Agent agent = new Agent(this, agentBehaviour);
            agent.Initialize();
            this.agents.Add(agent);
        }

        public void Update(float deltaTime)
        {
            time.Update(deltaTime);
            foreach (Agent agent in agents)
                agent.Update();
        }
    }
}