using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReachStatisticDefinition", menuName = "Definition/Statistic/ReachStatisticDefinition")]
    public class ReachStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.Reach;
        }
    }
}
