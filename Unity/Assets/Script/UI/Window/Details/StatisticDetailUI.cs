using TMPro;
using UnityEngine;

namespace Game
{
    public class StatisticDetailUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TextMeshProUGUI modifierValue;
        [SerializeField] private StatisticDefinition definition;

        public void Refresh(Character character)
        {
            float baseValue = 0;
            float total = 0;
            if (definition == StatisticDefinition.AttackPower)
            {
                baseValue = character.Definition.AttackPower;
                total = character.AttackPower;
            }
            else if (definition == StatisticDefinition.MaxHealth)
            {
                baseValue = character.Definition.MaxHealth;
                total = character.MaxHealth;
            }
            else if (definition == StatisticDefinition.Speed)
            {
                baseValue = character.Definition.Speed;
                total = character.Speed;
            }
            else if (definition == StatisticDefinition.AttackSpeed)
            {
                baseValue = character.Definition.AttackSpeed;
                total = character.AttackSpeed;
            }
            else if (definition == StatisticDefinition.Defense)
            {
                baseValue = character.Definition.Defense;
                total = character.Defense;
            }
            else if (definition == StatisticDefinition.Reach)
            {
                baseValue = character.Definition.Reach;
                total = character.Reach;
            }

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
