using AgeOfWarriors.Core;

namespace AgeOfWarriors
{
    public interface IDefinitionRepository
    {
        public ICharacterDefinition ShieldbearerDefinition { get; }

        public T Get<T>(string id)
            where T : IDefinition;
    }
}
