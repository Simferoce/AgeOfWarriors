using TMPro;
using UnityEngine;

namespace Game
{
    public class AbilityInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private StatisticReference<float> cooldown;

        public static AbilityInspectorWindow Open(Ability ability, string abilitySlotName)
        {
            AbilityInspectorWindow abilityInspectorWindow = WindowManager.Instance.GetWindow<AbilityInspectorWindow>();
            abilityInspectorWindow.Show();
            abilityInspectorWindow.title.text = ability.Definition.Title;
            abilityInspectorWindow.description.text = ability.Definition.ParseDescription(ability);

            if (abilityInspectorWindow.cooldown.TryGetValue(ability, out float value))
            {
                abilityInspectorWindow.cooldownText.alpha = 1f;
                abilityInspectorWindow.cooldownText.text = $"{value}{abilityInspectorWindow.cooldown.Definition.GetTextIcon}";
            }
            else
            {
                abilityInspectorWindow.cooldownText.alpha = 0f;
            }

            return abilityInspectorWindow;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
