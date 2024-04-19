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

        public void Refresh(Modifier modifier)
        {
            icon.sprite = modifier.Definition.Icon;
            icon.fillAmount = modifier.GetPercentageRemainingDuration() ?? 1;

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
