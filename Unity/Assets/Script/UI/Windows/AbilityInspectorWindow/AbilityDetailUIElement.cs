using Game.Ability;
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

        private AbilityEntity characterAbility;

        public void Refresh(AbilityEntity characterAbility)
        {
            this.characterAbility = characterAbility;
            slotNameText.text = slotName;
        }

        public void Inspect()
        {
            AbilityInspectorWindow.Open(characterAbility, slotName);
        }
    }
}
