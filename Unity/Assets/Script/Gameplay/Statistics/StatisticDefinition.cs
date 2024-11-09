using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game.Statistics
{
    public abstract class StatisticDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string humanReadableId;
        [SerializeField] private string title;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private bool isPercentage = false;
        [SerializeReference, SubclassSelector] private List<StatisticDefinitionData> data;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => ColorUtility.ToHtmlStringRGBA(color);
        public Color Color => color;
        public string TextIcon => Icon != null ? $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>" : "";
        public bool IsPercentage => isPercentage;
        public string HumanReadableId => humanReadableId;
        public List<StatisticDefinitionData> Data { get => data; set => data = value; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (icon == null)
            {
                string[] guids = AssetDatabase.FindAssets("DefaultStatisticIcon t:Sprite");
                string assetPath = guids.Select(x => AssetDatabase.GUIDToAssetPath(x)).FirstOrDefault();
                icon = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }

    public abstract class StatisticDefinition<T> : StatisticDefinition
    {
        public abstract T Modify(T value, StatisticRepository repository);
    }
}
