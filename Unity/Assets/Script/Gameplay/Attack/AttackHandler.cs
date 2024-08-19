using System.Collections.Generic;
using UnityEngine;
using static Game.ShieldModifierDefinition;

namespace Game
{
    public static class AttackHandler
    {
        public struct Input
        {
            public IAttackable Attacked { get; set; }
            public float CurrentHealth { get; set; }
            public float Defense { get; set; }
            public float IncreaseDamageTaken { get; set; }
            public float RangedDamageReduction { get; set; }
            public List<Shield> Shields { get; set; }

            public bool CanResistDeath { get; set; }

            public Input(IAttackable attacked, float currentHealth = 0, float defense = 0, float increaseDamageTaken = 0, float rangedDamageReduction = 0, List<Shield> shields = null, bool canResistDeath = false)
            {
                Attacked = attacked;
                CurrentHealth = currentHealth;
                Defense = defense;
                IncreaseDamageTaken = increaseDamageTaken;
                RangedDamageReduction = rangedDamageReduction;
                Shields = shields;
                CanResistDeath = canResistDeath;
            }
        }

        public struct Result
        {
            public float DamageToTake { get; set; }
            public float DamageAbsorbed { get; set; }
            public float DefenseDamagePrevented { get; set; }

            public bool ResistedDeath { get; set; }
        }

        public static Result TakeAttack(Attack attack, Input input)
        {
            float damage = attack.Damage;
            float resultingDefense = input.Defense - attack.ArmorPenetration;

            float damageTakenModifier = -input.IncreaseDamageTaken;
            if (attack.Ranged)
                damageTakenModifier += input.RangedDamageReduction;

            damageTakenModifier = Mathf.Clamp(1 - damageTakenModifier, 0.35f, 1.65f);
            damage *= damageTakenModifier;

            float damageRemaining = damage;
            if (!attack.OverTime)
                damageRemaining *= (1 / (1 + resultingDefense * 0.1f));

            float defenseDamagePrevented = damage - damageRemaining;

            if (input.Shields != null)
                damageRemaining = Absorb(damageRemaining, input.Shields);

            bool resistedDeath = false;
            if (input.CanResistDeath && input.CurrentHealth - damageRemaining <= 0)
                resistedDeath = true;

            return new Result()
            {
                DamageAbsorbed = damage - damageRemaining,
                DamageToTake = damageRemaining,
                ResistedDeath = resistedDeath,
                DefenseDamagePrevented = defenseDamagePrevented,
            };
        }

        public static float Absorb(float damageRemaining, List<Shield> shields)
        {
            for (int i = 0; i < shields.Count; i++)
                shields[i].Absorb(damageRemaining, out damageRemaining);

            return damageRemaining;
        }
    }
}