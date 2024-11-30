using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Statistics
{
    public class StatisticDefinitionRegistry : Manager<StatisticDefinitionRegistry>
    {
        public StatisticDefinition Health => statisticDefinitions["health"];
        public StatisticDefinition MaxHealth => statisticDefinitions["max_health"];
        public StatisticDefinition Defense => statisticDefinitions["defense"];
        public StatisticDefinition FlatDefense => statisticDefinitions["flat_defense"];
        public StatisticDefinition AttackPower => statisticDefinitions["attack_power"];
        public StatisticDefinition AttackSpeed => statisticDefinitions["attack_speed"];
        public StatisticDefinition Speed => statisticDefinitions["speed"];
        public StatisticDefinition Reach => statisticDefinitions["reach"];
        public StatisticDefinition Cooldown => statisticDefinitions["cooldown"];
        public StatisticDefinition Stagger => statisticDefinitions["stagger"];
        public StatisticDefinition DamageReduction => statisticDefinitions["damage_reduction"];
        public StatisticDefinition PercentageDamage => statisticDefinitions["percentage_damage"];

        private Dictionary<string, StatisticDefinition> statisticDefinitions = new Dictionary<string, StatisticDefinition>();
        private AsyncOperationHandle<IList<StatisticDefinition>> statisticsHandle;

        public override IEnumerator InitializeAsync()
        {
            statisticsHandle = Addressables.LoadAssetsAsync<StatisticDefinition>("Statistic", (StatisticDefinition statisticDefinition) => statisticDefinitions.Add(statisticDefinition.HumanReadableId, statisticDefinition));
            yield return statisticsHandle;
        }

        private void OnDestroy()
        {
            if (statisticsHandle.IsValid())
                Addressables.Release(statisticsHandle);
        }
    }
}