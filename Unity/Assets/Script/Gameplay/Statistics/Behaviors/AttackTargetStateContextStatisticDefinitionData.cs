using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class AttackTargetStateContextStatisticDefinitionData : ContextStatisticDefinitionData
    {
        [SerializeField] private StatisticDefinition<bool> definition;

        public override bool IsApplicable(Context context)
        {
            if (context is not AttackContext attackContext)
                return false;

            return attackContext.Target != null
                && attackContext.Target.Entity.TryGetCachedComponent<StatisticRepository>(out StatisticRepository statisticRepository)
                && statisticRepository.TryGet(definition, out Statistic<bool> statistic)
                && statistic.GetModifiedValue(context);
        }
    }
}
