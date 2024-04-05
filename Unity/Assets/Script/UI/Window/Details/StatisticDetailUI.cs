using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatisticDetailUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private StatisticDefinition statisticDefinition;

        public void Refresh(AgentObject agentObject)
        {
            icon.sprite = statisticDefinition.Icon;
            label.text = statisticDefinition.Title;
            value.text = statisticDefinition.GetFormatedValue(agentObject);
        }
    }
}
