using System.Collections.Generic;
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        public enum OperationType
        {
            Base,
            Flat,
            Percentage,
            Multiplier,
            Maximum,
            Minimum
        }

        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private List<StatisticDefinition> modifiers;
        [SerializeField] private OperationType operationType = OperationType.Base;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => ColorUtility.ToHtmlStringRGBA(color);
        public Color Color => color;
        public string TextIcon => Icon != null ? $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>" : "";
        public bool IsPercentage { get => operationType == OperationType.Percentage; }
        public OperationType Operation => operationType;
        public List<StatisticDefinition> Modifiers { get => modifiers; set => modifiers = value; }
    }
}
