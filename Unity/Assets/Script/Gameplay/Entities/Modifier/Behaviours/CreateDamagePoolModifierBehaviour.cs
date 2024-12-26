using Game.Agent;
using Game.Components;
using Game.Pool;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class CreateDamagePoolModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        [SerializeField] private GameObject poolPrefab;
        [SerializeField] private ModifierTarget target;
        [SerializeField] private StatisticReference duration;
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference threshold;

        public float CurrentStack { get; set; }

        private AttackFactory attackFactory;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackFactory = modifier.Target.Entity.GetCachedComponent<AttackFactory>();
            attackFactory.OnAttackLanded += OnAttackLanded;

            duration.Initialize(modifier);
            damage.Initialize(modifier);
            threshold.Initialize(modifier);
        }

        private void OnAttackLanded(AttackResult attackResult)
        {
            if (attackResult.AttackData.Source.Entity is PoolEntity)
                return;

            CurrentStack++;

            if (CurrentStack >= threshold.Get())
            {
                CurrentStack = 0;
                AgentIdentity ownerIdentity = modifier.Target.Entity.GetCachedComponent<AgentIdentity>();

                GameObject pool = GameObject.Instantiate(poolPrefab, Lane.Instance.Project(attackResult.Target.transform.position), Quaternion.identity);
                PoolEntity poolEntity = pool.GetComponent<PoolEntity>();
                poolEntity.StatisticRepository.Add(new StatisticFloat("duration", null, duration.Get()));
                poolEntity.StatisticRepository.Add(new StatisticFloat("damage", null, damage.Get()));

                AgentIdentity agentIdentity = poolEntity.AddOrGetCachedComponent<AgentIdentity>();
                agentIdentity.Agent = ownerIdentity.Agent;
                agentIdentity.SpawnNumber = int.MaxValue;

                poolEntity.Initialize(modifier.Target.Entity);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            attackFactory.OnAttackLanded -= OnAttackLanded;
        }
    }
}
