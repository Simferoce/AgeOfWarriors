﻿using Game.Agent;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class LevelInspectorWindow : Window
    {
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private CommanderDescriptionUIElement commanderDescription;
        [SerializeField] private TextMeshProUGUI description;

        private LevelDefinition levelDefinition;
        private List<LevelObjectiveUIElement> levelObjectives;

        private void Awake()
        {
            levelObjectives = GetComponentsInChildren<LevelObjectiveUIElement>(true).ToList();
        }

        public void Show(LevelDefinition levelDefinition)
        {
            base.Show();
            Refresh(levelDefinition);
        }

        public void Refresh(LevelDefinition levelDefinition)
        {
            this.levelDefinition = levelDefinition;
            levelName.text = $"-{levelDefinition.Title}-";

            levelObjectives[0].Refresh(levelDefinition.Objectives.Count > 0 ? levelDefinition.Objectives[0] : null);
            levelObjectives[1].Refresh(levelDefinition.Objectives.Count > 1 ? levelDefinition.Objectives[1] : null);
            levelObjectives[2].Refresh(levelDefinition.Objectives.Count > 2 ? levelDefinition.Objectives[2] : null);

            description.text = levelDefinition.Description;
            commanderDescription.Refresh(levelDefinition.Loadout.CommanderDefinition);
        }

        public void ChooseLoadout()
        {
            LoadoutSelectionWindow loadoutSelectionWindow = WindowManager.Instance.GetWindow<LoadoutSelectionWindow>();
            loadoutSelectionWindow.Show(new Agent.AgentLoadout() { CommanderDefinition = levelDefinition.Loadout.CommanderDefinition });
            loadoutSelectionWindow.OnLoadoutChoosen += OnLoadoutChoosen;

            void OnLoadoutChoosen(AgentLoadout agentLoadout)
            {
                loadoutSelectionWindow.OnLoadoutChoosen -= OnLoadoutChoosen;
                GameFlowManager.Instance.LoadLevel(levelDefinition, agentLoadout);
                Hide();
            }
        }

        public void Close()
        {
            Hide();
        }
    }
}
