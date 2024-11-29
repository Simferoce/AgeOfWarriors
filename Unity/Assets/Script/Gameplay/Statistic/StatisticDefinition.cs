using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StatisticDefinition", menuName = "Definition/Statistic")]
    public class StatisticDefinition : Definition
    {
        private static StatisticDefinition attackPower;
        private static StatisticDefinition attackPowerFlat;
        private static StatisticDefinition reach;
        private static StatisticDefinition percentageReach;
        private static StatisticDefinition health;
        private static StatisticDefinition maxHealth;
        private static StatisticDefinition flatMaxHealth;
        private static StatisticDefinition defense;
        private static StatisticDefinition flatDefense;
        private static StatisticDefinition attackSpeed;
        private static StatisticDefinition percentageAttackSpeed;
        private static StatisticDefinition speed;
        private static StatisticDefinition percentageSpeed;
        private static StatisticDefinition damage;
        private static StatisticDefinition range;
        private static StatisticDefinition duration;
        private static StatisticDefinition cooldown;
        private static StatisticDefinition heal;
        private static StatisticDefinition shield;
        private static StatisticDefinition rangedPercentageDamageTaken;
        private static StatisticDefinition percentageDamageTaken;
        private static StatisticDefinition percentageDamageDealt;
        private static StatisticDefinition percentageDamageDealtAgainstWeakTarget;
        private static StatisticDefinition technologyPerSeconds;

        public static StatisticDefinition AttackPower => attackPower ??= Resources.Load<StatisticDefinition>("Definition/Statistic/AttackPowerStatisticDefinition");
        public static StatisticDefinition FlatAttackPower => attackPowerFlat ??= Resources.Load<StatisticDefinition>("Definition/Statistic/FlatAttackPowerStatisticDefinition");
        public static StatisticDefinition Reach => reach ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ReachStatisticDefinition");
        public static StatisticDefinition PercentageReach => percentageReach ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageReachStatisticDefinition");
        public static StatisticDefinition Health => health ??= Resources.Load<StatisticDefinition>("Definition/Statistic/HealthStatisticDefinition");
        public static StatisticDefinition MaxHealth => maxHealth ??= Resources.Load<StatisticDefinition>("Definition/Statistic/MaxHealthStatisticDefinition");
        public static StatisticDefinition FlatMaxHealth => flatMaxHealth ??= Resources.Load<StatisticDefinition>("Definition/Statistic/FlatMaxHealthStatisticDefinition");
        public static StatisticDefinition Defense => defense ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DefenseStatisticDefinition");
        public static StatisticDefinition FlatDefense => flatDefense ??= Resources.Load<StatisticDefinition>("Definition/Statistic/FlatDefenseStatisticDefinition");
        public static StatisticDefinition AttackSpeed => attackSpeed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/AttackSpeedStatisticDefinition");
        public static StatisticDefinition PercentageAttackSpeed => percentageAttackSpeed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageAttackSpeedStatisticDefinition");
        public static StatisticDefinition Speed => speed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/SpeedStatisticDefinition");
        public static StatisticDefinition PercentageSpeed => percentageSpeed ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageSpeedStatisticDefinition");
        public static StatisticDefinition Damage => damage ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DamageStatisticDefinition");
        public static StatisticDefinition Range => range ??= Resources.Load<StatisticDefinition>("Definition/Statistic/RangeStatisticDefinition");
        public static StatisticDefinition Duration => duration ??= Resources.Load<StatisticDefinition>("Definition/Statistic/DurationStatisticDefinition");
        public static StatisticDefinition Cooldown => cooldown ??= Resources.Load<StatisticDefinition>("Definition/Statistic/CooldownStatisticDefinition");
        public static StatisticDefinition Heal => heal ??= Resources.Load<StatisticDefinition>("Definition/Statistic/HealStatisticDefinition");
        public static StatisticDefinition Shield => shield ??= Resources.Load<StatisticDefinition>("Definition/Statistic/ShieldStatisticDefinition");
        public static StatisticDefinition RangedPercentageDamageTaken => rangedPercentageDamageTaken ??= Resources.Load<StatisticDefinition>("Definition/Statistic/RangedPercentageDamageTakenStatisticDefinition");
        public static StatisticDefinition PercentageDamageDealtAgainstWeak => percentageDamageDealtAgainstWeakTarget ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageDamageDealtAgainstWeakStatisticDefinition");
        public static StatisticDefinition PercentageDamageDealt => percentageDamageDealt ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageDamageDealtStatisticDefinition");
        public static StatisticDefinition PercentageDamageTaken => percentageDamageTaken ??= Resources.Load<StatisticDefinition>("Definition/Statistic/PercentageDamageTakenStatisticDefinition");
        public static StatisticDefinition TechnologyPerSeconds => technologyPerSeconds ??= Resources.Load<StatisticDefinition>("Definition/Statistic/TechnologyPerSecondsStatisticDefinition");

        private static List<StatisticDefinition> all = null;
        public static List<StatisticDefinition> All => all ??= new List<StatisticDefinition>()
        {
            AttackPower,
            FlatAttackPower,
            Reach,
            PercentageReach,
            Health,
            MaxHealth,
            FlatMaxHealth,
            Defense,
            FlatDefense,
            AttackSpeed,
            PercentageAttackSpeed,
            Speed,
            PercentageSpeed,
            Damage,
            Range,
            Duration,
            Cooldown,
            Heal,
            Shield,
            RangedPercentageDamageTaken,
            PercentageDamageDealtAgainstWeak ,
            PercentageDamageDealt,
            PercentageDamageTaken ,
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
        public string TextIcon => $"<sprite name=\"{Icon.name.Trim()}\" color=#{ColorHex}>";
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
