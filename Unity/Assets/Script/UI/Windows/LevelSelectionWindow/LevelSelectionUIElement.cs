using UnityEngine;

namespace Game.UI.Windows
{
    public class LevelSelectionUIElement : MonoBehaviour
    {
        [SerializeField] private LevelDefinition levelDefinition;

        public void Select()
        {
            LevelInspectorWindow levelInspectorWindow = WindowManager.Instance.GetWindow<LevelInspectorWindow>();
            levelInspectorWindow.Show(levelDefinition);
        }
    }
}
