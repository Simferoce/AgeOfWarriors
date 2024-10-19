namespace Game.Statistics
{
    public interface IStatisticDefinitionProvider
    {
        public abstract StatisticDefinition GetById(StatisticIdentifiant identifiant);
    }
}
