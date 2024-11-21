using AgeOfWarriors.Core;
using AgeOfWarriors.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AgeOfWarriors
{
    public class Game
    {
        public Time Time { get => time; }
        public List<Entity> Entities { get => entities; set => entities = value; }
        public IGameDebug Debug { get => gameDebug; }
        public IDefinitionRepository DefinitionRepository { get => definitionRepository; }
        public EventChannel EventChannel { get => eventChannel; }
        public Lane Lane { get; set; }

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

            Lane = new Lane(this, 10);

            Base agentBase = new Base(this, Lane.GetMinimum(), QuaternionUtility.Right);
            Agent agent = new Agent(this, new AIAgentBehaviour());
            agentBase.Initialize();
            agent.Initialize(agentBase);

            Base agentBase2 = new Base(this, Lane.GetMaximum(), QuaternionUtility.Left);
            Agent agent2 = new Agent(this, new AIAgentBehaviour());
            agentBase2.Initialize();
            agent2.Initialize(agentBase2);


            this.agents.Add(agent);
        }

        public void Update(float deltaTime)
        {
            time.Update(deltaTime);
            foreach (Entity entity in entities.ToList())
                entity.Update();
        }
    }
}