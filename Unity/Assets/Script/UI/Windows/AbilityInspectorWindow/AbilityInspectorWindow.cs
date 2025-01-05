using Game.Statistics;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class AbilityInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI cooldownText;

        public void Show(IAbilityInspectable ability)
        {
            base.Show();
            title.text = ability.GetTitle();
            description.text = ability.GetDescription();

            float cooldown = ability.GetCooldown();
            if (cooldown > 0f)
            {
                cooldownText.alpha = 1f;
                cooldownText.text = $"{cooldown}{StatisticDefinitionRegistry.Instance.Cooldown.TextIcon}";
            }
            else
            {
                cooldownText.alpha = 0f;
            }
        }

        public void Close()
        {
            Hide();
        }
    }
}
