using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ModifierInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI modifierName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image icon;

        public static ModifierInspectorWindow Open(Modifier modifier)
        {
            ModifierInspectorWindow modifierInspectorWindow = WindowManager.Instance.GetWindow<ModifierInspectorWindow>();
            modifierInspectorWindow.Refresh(modifier);
            modifierInspectorWindow.Show();

            return modifierInspectorWindow;
        }

        public void Refresh(Modifier modifier)
        {
            modifierName.text = modifier.ModifierDefinition.Title;
            description.text = modifier.ModifierDefinition.ParseDescription(modifier);
            icon.sprite = modifier.ModifierDefinition.Icon;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
