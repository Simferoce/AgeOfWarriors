using AgeOfWarriors.Core;

namespace AgeOfWarriors
{
    public interface IDefinitionRepository
    {
        public T Get<T>(string id)
            where T : IDefinition;
    }
}
