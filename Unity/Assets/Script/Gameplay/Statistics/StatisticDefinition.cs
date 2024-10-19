using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private Color color;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => ColorUtility.ToHtmlStringRGBA(color);
        public Color Color => color;
        public string TextIcon => $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>";
    }
}
