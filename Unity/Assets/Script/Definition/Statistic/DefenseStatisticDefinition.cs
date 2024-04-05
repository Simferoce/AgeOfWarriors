using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseStatisticDefinition", menuName = "Definition/Statistic/DefenseStatisticDefinition")]
    public class DefenseStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.Defense;
        }
    }
}
