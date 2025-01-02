using Game.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CommanderSelectionUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;

        public delegate void OnSelectCommanderDelegate(CommanderSelectionUIElement commanderSelectionUIElement);
        public event OnSelectCommanderDelegate OnSelectCommander;

        public CommanderDefinition CommanderDefinition { get; private set; }

        public void Refresh(CommanderDefinition commanderDefinition, bool isSelected)
        {
            CommanderDefinition = commanderDefinition;

            if (commanderDefinition == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = commanderDefinition.Icon;
                background.color = isSelected ? WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red) : WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple) * WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red);
            }
        }

        public void OnSelect()
        {
            OnSelectCommander?.Invoke(this);
        }
    }
}
