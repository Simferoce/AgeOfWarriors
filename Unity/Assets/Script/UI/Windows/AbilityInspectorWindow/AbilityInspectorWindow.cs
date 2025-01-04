using Game.Ability;
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

        public void Show(AbilityEntity ability, string abilitySlotName)
        {
            base.Show();
            title.text = ability.GetDefinition().Title;
            description.text = ability.ParseDescription();

            float cooldown = 0f;//ability.GetCachedComponent<StatisticIndex>().Max(StatisticIdentifiant.Cooldown);
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
