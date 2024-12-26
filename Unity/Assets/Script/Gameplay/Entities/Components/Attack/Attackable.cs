using Game.Statistics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class Attackable : MonoBehaviour
    {
        public delegate void OnDamageTakenDelegate(AttackResult result, Attackable receiver);
        public event OnDamageTakenDelegate OnDamageTaken;
        public delegate void OnDestroyDelegate(Attackable attackable);
        public event OnDestroyDelegate OnDeactivated;
        public delegate void OnPotentialDeathDelegate(AttackData attack, ref bool resist);
        public event OnPotentialDeathDelegate OnPotentialDeath;

        public Entity Entity { get; set; }
        public float LastTimeAttacked { get; private set; }
        public ShieldHandler ShieldHandler { get; } = new ShieldHandler();

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
            bool invulnerable = Entity.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.Invulnerable, out Statistic invulnerableStatistic) ? invulnerableStatistic.Get<bool>() : false;
            //List<Shield> shields = Entity.GetCachedComponent<ModifierHandler>().GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList();

            bool resistedDeath = false;
            float defenseDamagePrevented = 0f;
            float damageRemaining = 0f;
            if (!invulnerable)
            {
                float damage = attack.Damage;
                if (attack.Flags.HasFlag(AttackData.Flag.Empowered))
                    damage *= 1.5f;

                if (attack.Flags.HasFlag(AttackData.Flag.Ranged) && Entity.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.RangeDamageTaken, out Statistic rangeDamageTakenStatistic))
                    damage *= rangeDamageTakenStatistic.Get<float>();

                if (Entity.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.DamageTaken, out Statistic damageTakenStatistic))
                    damage *= damageTakenStatistic.Get<float>();

                float resultingDefense = currentDefense - attack.ArmorPenetration;
                if (resultingDefense < 0f)
                    resultingDefense = 0f;

                damageRemaining = damage;
                if (!attack.Flags.HasFlag(AttackData.Flag.OverTime))
                    damageRemaining *= (1 / (1 + resultingDefense * 0.1f));

                defenseDamagePrevented = damage - damageRemaining;

                float damageBeforeAbsortion = damageRemaining;
                damageRemaining = ShieldHandler.Absorb(damageRemaining);

                float damageAbsorbed = damageBeforeAbsortion - damageRemaining;

                if (currentHealth - damageRemaining <= 0)
                {
                    OnPotentialDeath?.Invoke(attack, ref resistedDeath);

                    if (resistedDeath)
                        damageRemaining = currentHealth - 0.01f;
                }
            }

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