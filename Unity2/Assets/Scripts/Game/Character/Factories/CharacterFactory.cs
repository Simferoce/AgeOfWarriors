namespace AgeOfWarriors
{
    public abstract class CharacterFactory
    {
        protected ICharacterDefinition definition;

        protected CharacterFactory(ICharacterDefinition definition)
        {
            this.definition = definition;
        }

        public abstract Character Instantiate(Agent agent);
    }
}
