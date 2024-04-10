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
        [SerializeField] private StatisticReference<float> reference;

        public void Refresh(AgentObject agentObject)
        {
            StatisticDefinition definition = reference.Definition;

            icon.sprite = definition.Icon;
            label.text = definition.Title;
            value.text = reference.GetValueOrDefault(agentObject).ToString();
        }
    }
}
