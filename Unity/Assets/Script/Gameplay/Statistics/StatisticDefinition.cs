using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private bool isPercentage;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => ColorUtility.ToHtmlStringRGBA(color);
        public Color Color => color;
        public string TextIcon => Icon != null ? $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>" : "";
        public bool IsPercentage { get => isPercentage; set => isPercentage = value; }
    }
}
