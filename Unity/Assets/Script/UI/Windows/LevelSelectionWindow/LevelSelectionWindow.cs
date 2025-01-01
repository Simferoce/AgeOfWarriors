using UnityEngine;
namespace Game.UI.Windows
{
    public class LevelSelectionWindow : Window
    {
        [SerializeField] private LevelDefinition levelDefinition;

        [ContextMenu("Select")]
        private void TestSelectLevel()
        {
            GameFlowManager.Instance.LoadLevel(levelDefinition);
            this.Hide();
        }
    }
}