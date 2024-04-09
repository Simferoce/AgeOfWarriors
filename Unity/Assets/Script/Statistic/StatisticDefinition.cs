using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        private static StatisticDefinition attackPower;
        public static StatisticDefinition AttackPower
        {
            get
            {
                if (attackPower == null)
                    attackPower = Resources.Load<StatisticDefinition>("Definition/Statistic/AttackPowerStatisticDefinition");

                return attackPower;
            }
        }

        private static StatisticDefinition reach;
        public static StatisticDefinition Reach
        {
            get
            {
                if (reach == null)
                    reach = Resources.Load<StatisticDefinition>("Definition/Statistic/ReachStatisticDefinition");

                return reach;
            }
        }

        private static StatisticDefinition maxHealth;
        public static StatisticDefinition MaxHealth
        {
            get
            {
                if (maxHealth == null)
                    maxHealth = Resources.Load<StatisticDefinition>("Definition/Statistic/MaxHealthStatisticDefinition");

                return maxHealth;
            }
        }

        private static StatisticDefinition defense;
        public static StatisticDefinition Defense
        {
            get
            {
                if (defense == null)
                    defense = Resources.Load<StatisticDefinition>("Definition/Statistic/DefenseStatisticDefinition");

                return defense;
            }
        }

        private static StatisticDefinition attackSpeed;
        public static StatisticDefinition AttackSpeed
        {
            get
            {
                if (attackSpeed == null)
                    attackSpeed = Resources.Load<StatisticDefinition>("Definition/Statistic/AttackSpeedStatisticDefinition");

                return attackSpeed;
            }
        }

        private static StatisticDefinition speed;
        public static StatisticDefinition Speed
        {
            get
            {
                if (speed == null)
                    speed = Resources.Load<StatisticDefinition>("Definition/Statistic/SpeedStatisticDefinition");

                return speed;
            }
        }

        [SerializeField] private Sprite icon;
        [SerializeField] private string title;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
    }
}
