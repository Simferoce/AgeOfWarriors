using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class ModifierInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI modifierName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image icon;

        public void Show(IModifierInspectable modifier)
        {
            Refresh(modifier);
            TimeManager.Instance.SetTimeScale(this, 0f);
        }

        public void Refresh(IModifierInspectable modifier)
        {
            modifierName.text = modifier.GetTitle();
            description.text = modifier.GetDescription();
            icon.sprite = modifier.GetIcon();
        }

        public void Close()
        {
            Hide();

            TimeManager.Instance.ClearTimeScale(this);
        }
    }
}
