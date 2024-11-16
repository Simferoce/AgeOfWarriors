using AgeOfWarriors.Core;
using System.Collections.Generic;

namespace AgeOfWarriors
{
    public class Agent : Entity
    {
        public AgentCharacterFactory Factory { get => factory; }
        public AgentLoadout Loadout { get => loadout; }

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
            loadout = new AgentLoadout(this, new List<CharacterFactory>() { new ShieldbearerFactory(shieldbearerDefinition) });
        }

        public void AssignCharacter(Character character)
        {
            characters.Add(character);
        }

        public void Update()
        {
            behaviour.Update();
            factory.Update();
        }
    }
}