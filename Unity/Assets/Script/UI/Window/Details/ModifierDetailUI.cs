using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ModifierDetailUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private Modifier modifier;

        public void Refresh(Modifier modifier)
        {
            this.modifier = modifier;
            icon.sprite = modifier.ModifierDefinition.Icon;
            icon.fillAmount = modifier.GetPercentageRemaingDuration() ?? 1;
        }

        public void Inspect()
        {
            ModifierInspectorWindow.Open(modifier);
        }
    }
}
