using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        private static StatisticDefinition attackPower;
        private static StatisticDefinition reach;
        private static StatisticDefinition maxHealth;
        private static StatisticDefinition defense;
        private static StatisticDefinition attackSpeed;
        private static StatisticDefinition speed;
        private static StatisticDefinition damage;
        private static StatisticDefinition range;
        private static StatisticDefinition buffDuration;
        private static StatisticDefinition duration;
        private static StatisticDefinition cooldown;
        private static StatisticDefinition threshold;
        private static StatisticDefinition heal;
        private static StatisticDefinition shield;
        private static StatisticDefinition rangedDamageReduction;
        private static StatisticDefinition stack;
        private static StatisticDefinition damageIncrease;

        public static StatisticDefinition AttackPower => attackPower ??= Resources.Load<StatisticDefinition>("Definition/Statistic/AttackPowerStatisticDefinition");
        public static StatisticDefinition Reach => reach ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ReachStatisticDefinition");
        public static StatisticDefinition MaxHealth => maxHealth ??= Resources.Load<StatisticDefinition>("Definition/Statistic/MaxHealthStatisticDefinition");
        public static StatisticDefinition Defense => defense ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DefenseStatisticDefinition");
        public static StatisticDefinition AttackSpeed => attackSpeed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/AttackSpeedStatisticDefinition");
        public static StatisticDefinition Speed => speed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/SpeedStatisticDefinition");
        public static StatisticDefinition Damage => damage ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DamageStatisticDefinition");
        public static StatisticDefinition Range => range ??= Resources.Load<StatisticDefinition>("Definition/Statistic/RangeStatisticDefinition");
        public static StatisticDefinition BuffDuration => buffDuration ??= Resources.Load<StatisticDefinition>("Definition/Statistic/BuffDurationStatisticDefinition");
        public static StatisticDefinition Duration => duration ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DurationStatisticDefinition");
        public static StatisticDefinition Cooldown => cooldown ??= Resources.Load<StatisticDefinition>("Definition/Statistic/CooldownStatisticDefinition");
        public static StatisticDefinition Threshold => threshold ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ThresholdStatisticDefinition");
        public static StatisticDefinition Heal => heal ??= Resources.Load<StatisticDefinition>("Definition/Statistic/HealStatisticDefinition");
        public static StatisticDefinition Shield => shield ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ShieldStatisticDefinition");
        public static StatisticDefinition RangedDamageReduction => rangedDamageReduction ??= Resources.Load<StatisticDefinition>("Definition/Statistic/RangedDamageReductionStatisticDefinition");
        public static StatisticDefinition Stack => stack ??= Resources.Load<StatisticDefinition>("Definition/Statistic/StackStatisticDefinition");
        public static StatisticDefinition DamageIncrease => damageIncrease ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DamageIncreaseStatisticDefinition");

        private static List<StatisticDefinition> all = null;
        public static List<StatisticDefinition> All => all ??= new List<StatisticDefinition>()
        {
            AttackPower,
            Reach,
            MaxHealth,
            Defense,
            AttackSpeed,
            Speed,
            Damage,
            Range,
            BuffDuration,
            Duration,
            Cooldown,
            Threshold,
            Heal,
            Shield
        };

        [SerializeField] private Sprite icon;
        [SerializeField] private string humanReadableId;
        [SerializeField] private string title;
        [SerializeField] private Color color;

        public Sprite Icon => icon;
        public string Title => title;
        public string TitleFormatted => $"<color=#{ColorHex}>{Title}</color>";
        public string ColorHex => color.ToHexString();
        public Color Color => color;
        public string GetTextIcon => $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>";
        public string HumanReadableId => humanReadableId;

        public static string ParseText(string text)
        {
            string modifiedText = text;
            foreach (StatisticDefinition statisticDefinition in All)
            {
                modifiedText = modifiedText.Replace($"{{stat:{statisticDefinition.humanReadableId}}}", statisticDefinition.Title);
            }

            return modifiedText;
        }
    }
}
