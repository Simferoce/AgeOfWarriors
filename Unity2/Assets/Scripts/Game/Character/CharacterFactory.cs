using System.Numerics;

namespace AgeOfWarriors
{
    public class CharacterFactory
    {
        protected ICharacterDefinition definition;

        public CharacterFactory(ICharacterDefinition definition)
        {
            this.definition = definition;
        }

        public Character Instantiate(Agent agent, Vector2 position, Quaternion rotation)
        {
            return new Character(agent, definition, position, rotation);
        }
    }
}
