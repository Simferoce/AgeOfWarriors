using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.ShieldModifierDefinition;

namespace Game
{
    public class Attackable : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        public delegate void OnAttackTakenDelegate(AttackResult attackResult);
        public event OnAttackTakenDelegate OnAttackTaken;

        private void Awake()
        {
            this.Entity = GetComponentInParent<Entity>();
        }

        public void TakeAttack(Attack attack)
        {
            float defense = Entity[StatisticDefinition.Defense];
            float currentHealth = Entity[StatisticDefinition.Health];
            float increaseDamageTaken = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.DamageTaken, out Statistic<float> statistic) ? statistic.GetValue() : 0f);
            float rangedDamageReduction = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.RangedDamageTaken, out Statistic<float> statistic) ? statistic.GetValue() : 0f);
            List<Shield> shields = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList();
            bool canResistDeath = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());

            float damage = attack.Damage;
            float resultingDefense = defense - attack.ArmorPenetration;

            float damageTakenModifier = -increaseDamageTaken;
            if (attack.Ranged)
                damageTakenModifier += rangedDamageReduction;

            damageTakenModifier = Mathf.Clamp(1 - damageTakenModifier, 0.35f, 1.65f);
            damage *= damageTakenModifier;

            float damageRemaining = damage;
            if (!attack.OverTime)
                damageRemaining *= (1 / (1 + resultingDefense * 0.1f));

            float defenseDamagePrevented = damage - damageRemaining;

            if (shields != null)
                damageRemaining = Absorb(damageRemaining, shields);

            bool resistedDeath = false;
            if (canResistDeath && currentHealth - damageRemaining <= 0)
            {
                ResistKillingBlowPerk.Modifier modifier = (ResistKillingBlowPerk.Modifier)Entity.GetCachedComponent<ModifierHandler>().GetModifiers().FirstOrDefault(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());
                if (modifier != null)
                {
                    modifier.ResistKillingBlow();
                    resistedDeath = true;
                }
            }

            AttackResult result = new AttackResult(attack, damage - damageRemaining, defenseDamagePrevented, resistedDeath, this);
            OnAttackTaken?.Invoke(result);

            attack.AttackFactory.NotifyAttackLanded(result);
        }

        public float Absorb(float damageRemaining, List<Shield> shields)
        {
            for (int i = 0; i < shields.Count; i++)
                shields[i].Absorb(damageRemaining, out damageRemaining);

            return damageRemaining;
        }
    }
}