using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AbilityInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image icon;

        public static AbilityInspectorWindow Open(Ability ability)
        {
            AbilityInspectorWindow abilityInspectorWindow = WindowManager.Instance.GetWindow<AbilityInspectorWindow>();
            abilityInspectorWindow.Show();
            abilityInspectorWindow.title.text = ability.Definition.Title;
            abilityInspectorWindow.icon.sprite = ability.Definition.Icon;
            abilityInspectorWindow.description.text = ability.Definition.ParseDescription(ability);

            return abilityInspectorWindow;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
