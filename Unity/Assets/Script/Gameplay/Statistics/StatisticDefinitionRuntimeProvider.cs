using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Statistics
{
    public class StatisticDefinitionRuntimeProvider : Manager<StatisticDefinitionRuntimeProvider>, IStatisticDefinitionProvider
    {
        private Dictionary<string, StatisticDefinition> statisticDefinitions = new Dictionary<string, StatisticDefinition>();
        private AsyncOperationHandle<IList<StatisticDefinition>> statisticsHandle;

        public override IEnumerator InitializeAsync()
        {
            statisticsHandle = Addressables.LoadAssetsAsync<StatisticDefinition>("Statistic", (StatisticDefinition statisticDefinition) => statisticDefinitions.Add(statisticDefinition.Id, statisticDefinition));
            yield return statisticsHandle;
        }

        public StatisticDefinition GetById(StatisticIdentifiant identifiant)
        {
            string id = StatisticIdentifiantMap.GetReferenceId(identifiant);
            Assert.IsTrue(statisticDefinitions.ContainsKey(id), $"Unable to retreive the statistic definition with the identifiant {identifiant}.");

            return statisticDefinitions[id];
        }

        private void OnDestroy()
        {
            if (statisticsHandle.IsValid())
                Addressables.Release(statisticsHandle);
        }
    }
}