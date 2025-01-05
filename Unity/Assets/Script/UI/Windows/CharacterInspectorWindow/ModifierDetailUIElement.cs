using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class ModifierDetailUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject stack;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private Image overlay;

        private IModifierInspectable modifier;

        public void Refresh(IModifierInspectable modifier)
        {
            this.modifier = modifier;
            icon.sprite = modifier.GetIcon();
            overlay.fillAmount = modifier.GetPercentageRemainingDuration() ?? 0;

            float? stackValue = modifier.GetStack();
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

        public void Inspect()
        {
            ModifierInspectorWindow modifierInspectorWindow = WindowManager.Instance.GetWindow<ModifierInspectorWindow>();
            modifierInspectorWindow.Show(modifier);
        }
    }
}
