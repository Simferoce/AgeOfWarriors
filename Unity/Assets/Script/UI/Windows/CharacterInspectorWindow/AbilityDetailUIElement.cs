using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class AbilityDetailUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private string slotName;
        [SerializeField] private TextMeshProUGUI slotNameText;

        private IAbilityInspectable ability;

        public void Refresh(IAbilityInspectable characterAbility)
        {
            this.ability = characterAbility;
            slotNameText.text = slotName;
        }

        public void Inspect()
        {
            AbilityInspectorWindow abilityInspectorWindow = WindowManager.Instance.GetWindow<AbilityInspectorWindow>();
            abilityInspectorWindow.Show(ability);
        }
    }
}
