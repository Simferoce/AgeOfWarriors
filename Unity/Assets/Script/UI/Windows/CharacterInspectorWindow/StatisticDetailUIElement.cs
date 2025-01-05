using Game.Statistics;
using System;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class StatisticDetailUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TextMeshProUGUI modifierValue;
        [SerializeField] private StatisticDefinition definition;

        public void Refresh(ICharacterInspectable character)
        {
            float statisticValue = character.GetStatistic(definition);
            float baseValue = (float)Math.Round((double)statisticValue, 2);
            float total = (float)Math.Round((double)statisticValue, 2);
            float difference = (float)Math.Round((double)(total - baseValue), 2);

            label.text = definition.Title;
            value.text = total.ToString() + definition.TextIcon;

            ColorUtility.TryParseHtmlString("#00BF50", out Color positive);
            ColorUtility.TryParseHtmlString("#BF1D1D", out Color negative);
            modifierValue.color = difference >= 0 ? positive : negative;
            modifierValue.text = "(" + (difference >= 0 ? "+" : "-") + difference.ToString() + ")";
        }
    }
}
