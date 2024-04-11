using TMPro;
using UnityEngine;

namespace Game
{
    public class StatisticDetailUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TextMeshProUGUI modifierValue;
        [SerializeField] private StatisticReference<float> reference;

        public void Refresh(AgentObject agentObject)
        {
            float baseValue = reference.GetValueOrThrow(agentObject, StatisticType.Base);
            float total = reference.GetValueOrThrow(agentObject, StatisticType.Total);
            float difference = total - baseValue;

            StatisticDefinition definition = reference.Definition;

            label.text = definition.Title;
            value.text = total.ToString() + reference.Definition.GetTextIcon;

            ColorUtility.TryParseHtmlString("#00BF50", out Color positive);
            ColorUtility.TryParseHtmlString("#BF1D1D", out Color negative);
            modifierValue.color = difference >= 0 ? positive : negative;
            modifierValue.text = "(" + (difference >= 0 ? "+" : "-") + difference.ToString() + ")";
        }
    }
}
