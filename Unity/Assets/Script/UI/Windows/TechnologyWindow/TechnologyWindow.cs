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

        public void Show(AgentEntity agent)
        {
            base.Show();
            this.agent = agent;

            technologyLevelUI.Refresh(agent);
            technologySelectionTrees.Clear();
            for (int i = selectionElementContainer.transform.childCount - 1; i >= 0; --i)
                Destroy(selectionElementContainer.transform.GetChild(i).gameObject);

            foreach (TechnologyTree technologyTree in agent.Technology.TechnologyTrees)
            {
                GameObject selection = Instantiate(selectionElementPrefab, selectionElementContainer.transform);
                UITechnologySelectionTree technologySelectionTree = selection.GetComponent<UITechnologySelectionTree>();
                technologySelectionTree.Initialize(technologyTree);

                technologySelectionTree.OnSelect += TechnologySelectionTree_OnSelect;
                technologySelectionTrees.Add(technologySelectionTree);
            }

            technologySelectionTrees[0].Select();

            TimeManager.Instance.SetTimeScale(this, 0);
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
