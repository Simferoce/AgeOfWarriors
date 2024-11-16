using AgeOfWarriors.Core;
using System.Collections.Generic;

namespace AgeOfWarriors
{
    public class AgentLoadout : Entity
    {
        private List<CharacterFactory> characterFactories;

        public AgentLoadout(Agent agent, List<CharacterFactory> characterFactories)
            : base(agent.Game)
        {
            this.characterFactories = characterFactories;
        }

        public CharacterFactory GetCharacterFactoryAtIndex(int index)
        {
            return characterFactories[index];
        }
    }
}