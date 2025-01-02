using Game.Modifier;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CommanderModifierUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject stack;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private Image overlay;

        private ModifierEntity modifier;

        public void Refresh(ModifierEntity modifier)
        {
            this.modifier = modifier;

            icon.sprite = modifier.GetDefinition().Icon;
            overlay.fillAmount = modifier?.Behaviours.OfType<IModifierDuration>().FirstOrDefault()?.GetPercentageRemainingDuration() ?? 0;

            float? stackValue = modifier?.Behaviours.OfType<IModifierStack>().FirstOrDefault()?.CurrentStack;
            if (stackValue == null)
            {
                stack.SetActive(false);
            }
            else
            {
                stack.SetActive(true);
                stackText.text = stackValue.Value.ToString("N0");
            }
        }

        public void OnClick()
        {
            ModifierInspectorWindow.Open(modifier);
        }
    }
}
