using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedStatisticDefinition", menuName = "Definition/Statistic/SpeedStatisticDefinition")]
    public class SpeedStatisticDefinition : StatisticDefinition<float>
    {
        public override float GetValue(AgentObject agentObject)
        {
            return agentObject.Speed;
        }
    }
}
