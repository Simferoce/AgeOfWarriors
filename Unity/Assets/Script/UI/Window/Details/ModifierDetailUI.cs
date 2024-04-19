using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ModifierDetailUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject stack;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private Image overlay;

        private Modifier modifier;

        public void Refresh(Modifier modifier)
        {
            this.modifier = modifier;
            icon.sprite = modifier.Definition.Icon;
            overlay.fillAmount = modifier.GetPercentageRemainingDuration() ?? 0;

            float? stackValue = modifier.GetStack();
            if (stackValue == null)
            {
                stack.SetActive(false);
            }
            else
            {
                stack.SetActive(true);
                stackText.text = stackValue.ToString();
            }
        }

        public void Inspect()
        {
            ModifierInspectorWindow.Open(modifier);
        }
    }
}
