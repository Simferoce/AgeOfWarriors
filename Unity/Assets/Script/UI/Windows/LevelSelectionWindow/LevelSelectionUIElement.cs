using UnityEngine;

namespace Game.UI.Windows
{
    public class LevelSelectionUIElement : MonoBehaviour
    {
        [SerializeField] private LevelDefinition levelDefinition;

        public void Select()
        {
            LevelInspectorWindow.Open(levelDefinition);
        }
    }
}
