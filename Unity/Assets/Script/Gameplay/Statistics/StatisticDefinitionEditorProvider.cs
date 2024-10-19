#if UNITY_EDITOR

using UnityEditor;
using UnityEngine.Assertions;

namespace Game.Statistics
{
    public class StatisticDefinitionEditorProvider : IStatisticDefinitionProvider
    {
        public StatisticDefinition GetById(StatisticIdentifiant identifiant)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(StatisticIdentifiantMap.GetReferenceId(identifiant));
            Assert.IsFalse(string.IsNullOrEmpty(assetPath), $"Unable to retreive the asset associated with the identifiant: {identifiant}");
            return AssetDatabase.LoadAssetAtPath<StatisticDefinition>(assetPath);
        }
    }
}

#endif