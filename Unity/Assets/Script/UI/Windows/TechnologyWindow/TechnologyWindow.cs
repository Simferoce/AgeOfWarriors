using Game.Agent;
using Game.Technology;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Windows
{
    public class TechnologyWindow : Window
    {
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject selectionElementPrefab;
        [SerializeField] private GameObject selectionElementContainer;
        [SerializeField] private TechnologyLevelUIElement technologyLevelUI;

        private AgentEntity agent;
        private List<UITechnologySelectionTree> technologySelectionTrees = new List<UITechnologySelectionTree>();

        public static TechnologyWindow Open(AgentEntity agent)
        {
            TechnologyWindow technologyWindow = WindowManager.Instance.GetWindow<TechnologyWindow>();
            technologyWindow.agent = agent;

            technologyWindow.technologyLevelUI.Refresh(agent);

            technologyWindow.technologySelectionTrees.Clear();
            for (int i = technologyWindow.selectionElementContainer.transform.childCount - 1; i >= 0; --i)
                Destroy(technologyWindow.selectionElementContainer.transform.GetChild(i).gameObject);

            foreach (TechnologyTree technologyTree in agent.Technology.TechnologyTrees)
            {
                GameObject selection = Instantiate(technologyWindow.selectionElementPrefab, technologyWindow.selectionElementContainer.transform);
                UITechnologySelectionTree technologySelectionTree = selection.GetComponent<UITechnologySelectionTree>();
                technologySelectionTree.Initialize(technologyTree);

                technologySelectionTree.OnSelect += technologyWindow.TechnologySelectionTree_OnSelect;
                technologyWindow.technologySelectionTrees.Add(technologySelectionTree);
            }

            technologyWindow.technologySelectionTrees[0].Select();
            technologyWindow.Show();

            TimeManager.Instance.SetTimeScale(technologyWindow, 0);

            return technologyWindow;
        }

        private void TechnologySelectionTree_OnSelect(UITechnologySelectionTree technologySelectionTree)
        {
            for (int i = content.transform.childCount - 1; i >= 0; --i)
                Destroy(content.transform.GetChild(i).gameObject);

            foreach (UITechnologySelectionTree otherTechnologySelectionTree in technologySelectionTrees)
            {
                if (otherTechnologySelectionTree != technologySelectionTree)
                    otherTechnologySelectionTree.Deselect();
            }

            GameObject technologyPanelGameObject = Instantiate(technologySelectionTree.TechnologyTree.TechnologyTreeDefinition.TreeEditPanelPrefab, content.transform);
            UITechnologyPanelUIElement technologyPanel = technologyPanelGameObject.GetComponent<UITechnologyPanelUIElement>();
            technologyPanel.Initialize(technologySelectionTree.TechnologyTree);
        }

        public override void Hide()
        {
            base.Hide();
            TimeManager.Instance.ClearTimeScale(this);

            foreach (UITechnologySelectionTree technologySelectionTree in technologySelectionTrees)
                technologySelectionTree.OnSelect -= TechnologySelectionTree_OnSelect;
        }

        public void Close()
        {
            Hide();
        }
    }
}
