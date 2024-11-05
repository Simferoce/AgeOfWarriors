using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic/StatisticDefinition")]
    public class StatisticDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string humanReadableId;
        [SerializeField] private string title;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private bool isPercentage = false;
        [SerializeReference, SubclassSelector] private List<StatisticBehavior> behaviors;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => ColorUtility.ToHtmlStringRGBA(color);
        public Color Color => color;
        public string TextIcon => Icon != null ? $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>" : "";
        public bool IsPercentage => isPercentage;
        public string HumanReadableId => humanReadableId;
        public List<StatisticBehavior> Behaviors { get => behaviors; set => behaviors = value; }

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
        public float Modify(float value, StatisticRepository repository)
        {
            float flat = 0f;
            float percentage = 0f;
            float multiplier = 1f;
            float maximum = float.MaxValue;
            float minimum = float.MinValue;

            foreach (Statistic statistic in repository.Statistics)
            {
                ModifyStatisticBehavior modifyStatisticBehavior = statistic.Definition.Behaviors.OfType<ModifyStatisticBehavior>().FirstOrDefault(x => x.Definition == this);
                if (modifyStatisticBehavior == null)
                    continue;

                if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Flat)
                    flat += statistic.Value.GetValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Pecentage)
                    percentage += statistic.Value.GetValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Multiplier)
                    multiplier *= statistic.Value.GetValue<float>();
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Maximum)
                    maximum = Mathf.Min(maximum, statistic.Value.GetValue<float>());
                else if (modifyStatisticBehavior.StatisticOperator == StatisticOperator.Minimum)
                    minimum = Mathf.Max(minimum, statistic.Value.GetValue<float>());
            }

            return Mathf.Clamp((value + flat) * (1 + percentage) * multiplier, minimum, maximum);
        }
    }
}
