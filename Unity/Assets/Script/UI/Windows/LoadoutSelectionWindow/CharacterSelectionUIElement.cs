using Game.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CharacterSelectionUIElement : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void Refresh(CharacterDefinition characterDefinition)
        {
            icon.sprite = characterDefinition.Icon;
        }
    }
}
