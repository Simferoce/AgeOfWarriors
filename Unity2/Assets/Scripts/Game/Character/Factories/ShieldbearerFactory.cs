namespace AgeOfWarriors
{
    public class ShieldbearerFactory : CharacterFactory
    {
        public ShieldbearerFactory(ICharacterDefinition definition) : base(definition)
        {
        }

        public override Character Instantiate(Agent agent)
        {
            return new ShieldbearerCharacter(agent, definition);
        }
    }
}
