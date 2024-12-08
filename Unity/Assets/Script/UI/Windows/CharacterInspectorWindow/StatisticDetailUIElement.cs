using Game.Character;
using Game.Statistics;
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

        public void Refresh(CharacterEntity character)
        {
            float baseValue = character[definition].GetBase<float>();
            float total = character[definition].Get<float>();
            float difference = total - baseValue;

            label.text = definition.Title;
            value.text = total.ToString() + definition.TextIcon;

            ColorUtility.TryParseHtmlString("#00BF50", out Color positive);
            ColorUtility.TryParseHtmlString("#BF1D1D", out Color negative);
            modifierValue.color = difference >= 0 ? positive : negative;
            modifierValue.text = "(" + (difference >= 0 ? "+" : "-") + difference.ToString() + ")";
        }
    }
}
