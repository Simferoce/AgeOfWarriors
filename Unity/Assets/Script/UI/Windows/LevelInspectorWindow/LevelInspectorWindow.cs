using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class LevelInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image commanderIcon;

        private LevelDefinition levelDefinition;
        private List<LevelObjectiveUIElement> levelObjectives;

        private void Awake()
        {
            levelObjectives = GetComponentsInChildren<LevelObjectiveUIElement>().ToList();
        }

        public static LevelInspectorWindow Open(LevelDefinition levelDefinition)
        {
            LevelInspectorWindow levelInspectorWindow = WindowManager.Instance.GetWindow<LevelInspectorWindow>();
            levelInspectorWindow.Show();
            levelInspectorWindow.Refresh(levelDefinition);

            return levelInspectorWindow;
        }

        public void Refresh(LevelDefinition levelDefinition)
        {
            this.levelDefinition = levelDefinition;
            title.text = levelDefinition.Title;
            description.text = levelDefinition.Description;
            commanderIcon.sprite = levelDefinition.CommanderDefinition.Icon;

            levelObjectives[0].Refresh(levelDefinition.Objectives.Count > 0 ? levelDefinition.Objectives[0] : null);
            levelObjectives[1].Refresh(levelDefinition.Objectives.Count > 1 ? levelDefinition.Objectives[1] : null);
            levelObjectives[2].Refresh(levelDefinition.Objectives.Count > 2 ? levelDefinition.Objectives[2] : null);
        }

        public void StartLevel()
        {
            GameFlowManager.Instance.LoadLevel(levelDefinition);
            Hide();
        }

        public void Close()
        {
            Hide();
        }
    }
}
