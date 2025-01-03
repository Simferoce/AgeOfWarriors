using Game.Modifier;
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

        public static ModifierInspectorWindow Open(ModifierEntity modifier)
        {
            ModifierInspectorWindow modifierInspectorWindow = WindowManager.Instance.GetWindow<ModifierInspectorWindow>();
            modifierInspectorWindow.Refresh(modifier);
            modifierInspectorWindow.Show();

            TimeManager.Instance.SetTimeScale(modifierInspectorWindow, 0f);

            return modifierInspectorWindow;
        }

        public void Refresh(ModifierEntity modifier)
        {
            modifierName.text = modifier.GetDefinition().Title;
            description.text = modifier.ParseDescription();
            icon.sprite = modifier.GetDefinition().Icon;
        }

        public void Close()
        {
            Hide();

            TimeManager.Instance.ClearTimeScale(this);
        }
    }
}
