using Game.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CharacterSelectionUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private CharacterDefinition characterDefinition;

        public void Refresh(CharacterDefinition characterDefinition)
        {
            this.characterDefinition = characterDefinition;
            icon.sprite = characterDefinition.Icon;
        }

        public void OnClick()
        {
            CharacterInspectorWindow characterInspectorWindow = WindowManager.Instance.GetWindow<CharacterInspectorWindow>();
            characterInspectorWindow.Show(new CharacterDefintionInspectable(characterDefinition));
        }
    }
}
