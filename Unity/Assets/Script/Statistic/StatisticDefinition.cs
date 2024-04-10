using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        private static StatisticDefinition attackPower;
        public static StatisticDefinition AttackPower => attackPower ??= Resources.Load<StatisticDefinition>("Definition/Statistic/AttackPowerStatisticDefinition");

        private static StatisticDefinition reach;
        public static StatisticDefinition Reach => reach ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ReachStatisticDefinition");

        private static StatisticDefinition maxHealth;
        public static StatisticDefinition MaxHealth => maxHealth ??= maxHealth = Resources.Load<StatisticDefinition>("Definition/Statistic/MaxHealthStatisticDefinition");

        private static StatisticDefinition defense;
        public static StatisticDefinition Defense => defense ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DefenseStatisticDefinition");

        private static StatisticDefinition attackSpeed;
        public static StatisticDefinition AttackSpeed => attackSpeed ??= attackSpeed = Resources.Load<StatisticDefinition>("Definition/Statistic/AttackSpeedStatisticDefinition");

        private static StatisticDefinition speed;
        public static StatisticDefinition Speed => speed ??= speed = Resources.Load<StatisticDefinition>("Definition/Statistic/SpeedStatisticDefinition");

        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private Color color;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => color.ToHexString();
    }
}
