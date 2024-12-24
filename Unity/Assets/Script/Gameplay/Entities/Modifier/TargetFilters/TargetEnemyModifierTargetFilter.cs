using Game.Agent;
using Game.Statistics;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class TargetEnemyModifierTargetFilter : ModifierTargetFilter
    {
        public override bool Execute(Entity target)
        {
            return modifier.Target.Entity.StatisticRepository.TryGet("faction", out Statistic factionStatistic)
                && target.TryGetCachedComponent<AgentIdentity>(out AgentIdentity targetIdentity)
                && factionStatistic.Get<FactionType>() != targetIdentity.Faction;
        }
    }
}
