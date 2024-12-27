using Game.Agent;
using Game.Pool;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class CreateBuffPoolModifierEffect : ModifierEffect
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private ModifierDefinition definition;
        [SerializeField] private StatisticReference durationPool;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameterFactories;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            foreach (ModifierParameterFactory parameterFactory in parameterFactories)
                parameterFactory.Initialize(modifier);

            durationPool.Initialize(modifier);
        }

        public override void Execute()
        {
            AgentIdentity ownerIdentity = modifier.Target.Entity.GetCachedComponent<AgentIdentity>();

            GameObject pool = GameObject.Instantiate(prefab, Lane.Instance.Project(modifier.Target.transform.position), Quaternion.identity);
            PoolEntity poolEntity = pool.GetComponent<PoolEntity>();
            ModifierApplier modifierApplier = poolEntity.GetCachedComponent<ModifierApplier>();
            poolEntity.StatisticRepository.Add(new StatisticFloat("duration", null, durationPool.Get()));

            AgentIdentity agentIdentity = poolEntity.AddOrGetCachedComponent<AgentIdentity>();
            agentIdentity.Agent = ownerIdentity.Agent;
            agentIdentity.SpawnNumber = int.MaxValue;

            modifierApplier.Apply(definition, poolEntity.GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());

            poolEntity.Initialize(modifier.Target.Entity);
        }
    }
}
