using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedStatisticDefinition", menuName = "Definition/Statistic/AttackSpeedStatisticDefinition")]
    public class AttackSpeedStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.AttackSpeed;
        }
    }
}
