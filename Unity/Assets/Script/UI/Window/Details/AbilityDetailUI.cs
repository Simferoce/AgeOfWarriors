using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AbilityDetailUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private string slotName;
        [SerializeField] private TextMeshProUGUI slotNameText;

        private Ability characterAbility;

        public void Refresh(Ability characterAbility)
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
