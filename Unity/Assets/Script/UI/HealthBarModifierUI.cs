using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarModifierUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject stack;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private Image overlay;

        public void Refresh(Modifier modifier)
        {
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
    }
}
