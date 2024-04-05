using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackPowerStatisticDefinition", menuName = "Definition/Statistic/AttackPowerStatisticDefinition")]
    public class AttackPowerStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.AttackPower;
        }
    }
}
