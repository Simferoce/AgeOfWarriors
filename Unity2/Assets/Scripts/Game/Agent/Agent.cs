using AgeOfWarriors.Core;
using System.Collections.Generic;

namespace AgeOfWarriors
{
    public class Agent : Entity
    {
        public AgentCharacterFactory Factory { get => factory; }
        public AgentLoadout Loadout { get => loadout; }
        public Base Base { get; private set; }

        private AgentBehaviour behaviour;
        private AgentCharacterFactory factory;
        private AgentLoadout loadout;
        private List<Character> characters = new List<Character>();

        public Agent(Game game, AgentBehaviour behaviour)
            : base(game)
        {
            this.behaviour = behaviour;
            behaviour.Initialize(this);

            factory = new AgentCharacterFactory(this);

            ICharacterDefinition shieldbearerDefinition = game.DefinitionRepository.ShieldbearerDefinition;
            loadout = new AgentLoadout(this, new List<CharacterFactory>() { new CharacterFactory(shieldbearerDefinition) });
        }

        public void Initialize(Base agentBase)
        {
            this.Base = agentBase;

            this.Initialize();
        }

        public void AssignCharacter(Character character)
        {
            characters.Add(character);
        }

        public override void Update()
        {
            base.Update();
            behaviour.Update();
            factory.Update();
        }
    }
}