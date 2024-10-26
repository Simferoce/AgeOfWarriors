using System.Collections.Generic;
using UnityEngine;

namespace Game.Statistics
{
    public static class StatisticIdentifiantMap
    {
        private static IReadOnlyDictionary<StatisticIdentifiant, string> statisticReferenceIdentifiants { get; } = new Dictionary<StatisticIdentifiant, string>()
        {
            { StatisticIdentifiant.Damage, "00df8ac314188b945945353a3fbb705b" },
            { StatisticIdentifiant.DamagePercentage, "c44430788b4fe5a4f822561156a457af" },
            { StatisticIdentifiant.DamagePercentageAgainstWeak, "b036b5bdd1cc473499a87d48935769a2" },
            { StatisticIdentifiant.Health, "692bd90ecc8e43840a83aaa5c9e54022" },
            { StatisticIdentifiant.MaxHealth, "73d27da68cca71f4ebc2e071904b1f80" },
            { StatisticIdentifiant.MaxHealthFlat, "80313f8013c25d5429c444461ffe8b6d" },
            { StatisticIdentifiant.MaxHealthPercentage, "604f9fcc6fc7f4a4ba799319774f00f0" },
            { StatisticIdentifiant.Defense, "7f97419c07cf32d49ae612434c1fb6b8" },
            { StatisticIdentifiant.DefenseFlat, "06200c8604de2004e9d137aa559904e0" },
            { StatisticIdentifiant.Cooldown, "2825126f3d8ee3941b6ea8c4d91818b8" },
            { StatisticIdentifiant.Duration, "1ccc5b379e02eba4d9b26729c6ede2af" },
            { StatisticIdentifiant.BuffDuration, "cbdb2f244b5bcc947aa217190b37b0f5" },
            { StatisticIdentifiant.Range, "a2aa5c999dfb39e4a989f3c6b532ecbf" },
            { StatisticIdentifiant.Reach, "ace6cadc457dd1b4e90c5edc491000f1" },
            { StatisticIdentifiant.Leach, "e864fc16d0f0def478387446a9b41863" },
            { StatisticIdentifiant.AttackSpeed, "d71a191ae10eaf24f8e2f26ada07d54a" },
            { StatisticIdentifiant.AttackSpeedPercentage, "4625489d85014b746b361303b9541d43" },
            { StatisticIdentifiant.AttackPower, "ff6ee24adf3c8424aad04267addb363d" },
            { StatisticIdentifiant.AttackPowerPercentage, "a01a2864733b8e44c953827a131a98bd" },
            { StatisticIdentifiant.Speed, "b6e7acc220a4175489e13715c3932744" },
            { StatisticIdentifiant.SpeedPercentage, "7dc7968e82158fc43824701568177722" },
            { StatisticIdentifiant.Heal, "43442952713ba724d9f3d09e9d1abdf9" },
            { StatisticIdentifiant.DefensePercentage, "493109d50bb4d0d4f916891e33c067e1" },
            { StatisticIdentifiant.ArmorPenetration, "1687044977f8da6489e79d61a4fa60b9" },
            { StatisticIdentifiant.DamageMultiplier, "ffa9a2d75aa6c9f4cb8ed744120428fc" },
            { StatisticIdentifiant.DamageTakenPercentage, "3a8366e7cfe4d2f449af5017cbec8765" },
        };

        public static string GetReferenceId(StatisticIdentifiant identifiant)
        {
            if (!statisticReferenceIdentifiants.ContainsKey(identifiant))
            {
                Debug.LogError($"Unable to retreive the id of the identifiant {identifiant}.");
                return string.Empty;
            }

            return statisticReferenceIdentifiants[identifiant];
        }
    }
}
