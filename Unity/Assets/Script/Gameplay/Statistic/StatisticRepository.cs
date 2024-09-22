
using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatisticRepository
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        statisticDefinitions = null;
    }

    public static string Damage = "damage";
    public static string MaxHealth = "max_health";
    public static string Health = "health";
    public static string Defense = "defense";
    public static string Cooldown = "cooldown";
    public static string Duration = "duration";
    public static string BuffDuration = "buff_duration";
    public static string Range = "range";
    public static string Reach = "reach";
    public static string Leach = "leach";
    public static string AttackSpeed = "attack_speed";
    public static string AttackPower = "attack_power";
    public static string Speed = "speed";
    public static string Heal = "heal";

    public static List<StatisticDefinition> statisticDefinitions = null;

    public static StatisticDefinition GetDefinition(string name)
    {
        if (statisticDefinitions == null)
            statisticDefinitions = Resources.LoadAll<StatisticDefinition>("Definition/Statistic").ToList();

        foreach (StatisticDefinition statisticDefinition in statisticDefinitions)
        {
            if (statisticDefinition.HumanReadableId == name)
                return statisticDefinition;
        }

        throw new System.Exception($"Unable to find the statistic with the name {name}");
    }
}

