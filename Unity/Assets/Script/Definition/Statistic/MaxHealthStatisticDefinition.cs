using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "MaxHealthStatisticDefinition", menuName = "Definition/Statistic/MaxHealthStatisticDefinition")]
    public class MaxHealthStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.MaxHealth;
        }
    }
}
