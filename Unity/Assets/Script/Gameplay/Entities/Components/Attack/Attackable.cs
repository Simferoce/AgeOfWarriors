﻿using Game.Statistics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class Attackable : MonoBehaviour
    {
        public struct Input
        {
            public Attackable Attacked { get; set; }
            public float CurrentHealth { get; set; }
            public float Defense { get; set; }
            public float IncreaseDamageTaken { get; set; }
            public float RangedDamageReduction { get; set; }
            //public List<Shield> Shields { get; set; }

            public bool CanResistDeath { get; set; }

            public Input(Attackable attacked, float currentHealth = 0, float defense = 0, float increaseDamageTaken = 0, float rangedDamageReduction = 0, /*List<Shield> shields = null,*/ bool canResistDeath = false)
            {
                Attacked = attacked;
                CurrentHealth = currentHealth;
                Defense = defense;
                IncreaseDamageTaken = increaseDamageTaken;
                RangedDamageReduction = rangedDamageReduction;
                //Shields = shields;
                CanResistDeath = canResistDeath;
            }
        }

        public delegate void OnDamageTakenDelegate(AttackResult result, Attackable receiver);
        public event OnDamageTakenDelegate OnDamageTaken;

        public delegate void OnDestroyDelegate(Attackable attackable);
        public event OnDestroyDelegate OnDeactivated;

        public Entity Entity { get; set; }
        public float LastTimeAttacked { get; private set; }

        private List<AttackFactory> hasBeenAttackedBy = new List<AttackFactory>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public void TakeAttack(AttackData attack)
        {
            if (!hasBeenAttackedBy.Contains(attack.Source))
            {
                hasBeenAttackedBy.Add(attack.Source);
                attack.Source.OnDeactivated += AttackSourceOnDeactivated;
            }

            float currentHealth = Entity.GetCachedComponent<StatisticRepository>().GetOrThrow<float>(StatisticDefinitionRegistry.Instance.Health).Get<float>();
            float currentDefense = Entity.GetCachedComponent<StatisticRepository>().Get<float>(StatisticDefinitionRegistry.Instance.Defense)?.Get<float>() ?? 0f;
            //float damageReduction = Entity.GetCachedComponent<StatisticRepository>().Get<float>(StatisticDefinitionRegistry.Instance.DamageReduction)?.Get<float>() ?? 0f;
            //List<Shield> shields = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList();
            //bool canResistDeath = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());

            float damage = attack.Damage;
            if (attack.Flags.HasFlag(AttackData.Flag.Empowered))
                damage *= 1.5f;

            if (attack.Flags.HasFlag(AttackData.Flag.Ranged))
                damage *= Entity[StatisticDefinitionRegistry.Instance.RangeDamageTaken];

            float resultingDefense = currentDefense - attack.ArmorPenetration;
            if (resultingDefense < 0f)
                resultingDefense = 0f;

            float damageRemaining = damage;
            if (!attack.Flags.HasFlag(AttackData.Flag.OverTime))
                damageRemaining *= (1 / (1 + resultingDefense * 0.1f));

            float defenseDamagePrevented = damage - damageRemaining;

            //float damageBeforeAbsortion = damageRemaining;
            //for (int i = 0; i < shields.Count; i++)
            //    shields[i].Absorb(damageRemaining, out damageRemaining);

            //float damageAbsorbed = damageBeforeAbsortion - damageRemaining;

            bool resistedDeath = false;
            //if (canResistDeath && currentHealth - damageRemaining <= 0)
            //{
            //    resistedDeath = true;
            //    damageRemaining = currentHealth - 0.01f;
            //}

            AttackResult attackResult = new AttackResult(attack, damageRemaining, defenseDamagePrevented, damageRemaining >= currentHealth && !resistedDeath, this, resistedDeath);
            attack.Source.NotifyAttackResult(attackResult);

            OnDamageTaken?.Invoke(attackResult, this);
            LastTimeAttacked = Time.time;
        }

        private void AttackSourceOnDeactivated(AttackFactory attackFactory)
        {
            attackFactory.OnDeactivated -= AttackSourceOnDeactivated;
            hasBeenAttackedBy.Remove(attackFactory);
        }

        //public float Absorb(float damageRemaining, List<Shield> shields)
        //{
        //    for (int i = 0; i < shields.Count; i++)
        //        shields[i].Absorb(damageRemaining, out damageRemaining);

        //    return damageRemaining;
        //}
    }

}