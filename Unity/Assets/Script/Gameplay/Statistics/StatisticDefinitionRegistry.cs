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
        public StatisticDefinition FlatMaxHealth => statisticDefinitions["flat_max_health"];
        public StatisticDefinition Defense => statisticDefinitions["defense"];
        public StatisticDefinition FlatDefense => statisticDefinitions["flat_defense"];
        public StatisticDefinition AttackPower => statisticDefinitions["attack_power"];
        public StatisticDefinition FlatAttackPower => statisticDefinitions["flat_attack_power"];
        public StatisticDefinition AttackSpeed => statisticDefinitions["attack_speed"];
        public StatisticDefinition PercentageAttackSpeed => statisticDefinitions["percentage_attack_speed"];
        public StatisticDefinition MultiplierAttackSpeed => statisticDefinitions["multiplier_attack_speed"];
        public StatisticDefinition Speed => statisticDefinitions["speed"];
        public StatisticDefinition MultiplierSpeed => statisticDefinitions["multiplier_speed"];
        public StatisticDefinition Reach => statisticDefinitions["reach"];
        public StatisticDefinition Range => statisticDefinitions["range"];
        public StatisticDefinition Cooldown => statisticDefinitions["cooldown"];
        public StatisticDefinition Stagger => statisticDefinitions["stagger"];
        public StatisticDefinition Weak => statisticDefinitions["weak"];
        public StatisticDefinition DamageTaken => statisticDefinitions["damage_taken"];
        public StatisticDefinition RangeDamageTaken => statisticDefinitions["ranged_damage_taken"];
        public StatisticDefinition Damage => statisticDefinitions["damage"];
        public StatisticDefinition FlatDamageVersusWeak => statisticDefinitions["flat_damage_versus_weak"];
        public StatisticDefinition MultiplierDamage => statisticDefinitions["multiplier_damage"];
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