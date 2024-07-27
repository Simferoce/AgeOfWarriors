using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TechnologyWindow : Window
    {
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject selectionElementPrefab;
        [SerializeField] private GameObject selectionElementContainer;
        [SerializeField] private TechnologyLevelUI technologyLevelUI;

        private Agent agent;
        private List<TechnologySelectionTreeUI> technologySelectionTrees = new List<TechnologySelectionTreeUI>();

        public static TechnologyWindow Open(Agent agent)
        {
            TechnologyWindow technologyWindow = WindowManager.Instance.GetWindow<TechnologyWindow>();
            technologyWindow.agent = agent;

            technologyWindow.technologyLevelUI.Refresh(agent);

            technologyWindow.technologySelectionTrees.Clear();
            for (int i = technologyWindow.selectionElementContainer.transform.childCount - 1; i >= 0; --i)
                GameObject.Destroy(technologyWindow.selectionElementContainer.transform.GetChild(i).gameObject);

            foreach (TechnologyTree technologyTree in agent.Technology.TechnologyTrees)
            {
                GameObject selection = GameObject.Instantiate(technologyWindow.selectionElementPrefab, technologyWindow.selectionElementContainer.transform);
                TechnologySelectionTreeUI technologySelectionTree = selection.GetComponent<TechnologySelectionTreeUI>();
                technologySelectionTree.Initialize(technologyTree);

                technologySelectionTree.OnSelect += technologyWindow.TechnologySelectionTree_OnSelect;
                technologyWindow.technologySelectionTrees.Add(technologySelectionTree);
            }

            technologyWindow.technologySelectionTrees[0].Select();

            technologyWindow.Show();

            Time.timeScale = 0f;

            return technologyWindow;
        }

        private void TechnologySelectionTree_OnSelect(TechnologySelectionTreeUI technologySelectionTree)
        {
            for (int i = content.transform.childCount - 1; i >= 0; --i)
                GameObject.Destroy(content.transform.GetChild(i).gameObject);

            foreach (TechnologySelectionTreeUI otherTechnologySelectionTree in technologySelectionTrees)
            {
                if (otherTechnologySelectionTree != technologySelectionTree)
                    otherTechnologySelectionTree.Deselect();
            }

            GameObject technologyPanelGameObject = GameObject.Instantiate(technologySelectionTree.TechnologyTree.TechnologyTreeDefinition.TreeEditPanelPrefab, content.transform);
            TechnologyPanelUI technologyPanel = technologyPanelGameObject.GetComponent<TechnologyPanelUI>();
            technologyPanel.Initialize(technologySelectionTree.TechnologyTree);
        }

        public override void Hide(Result result)
        {
            base.Hide(result);
            Time.timeScale = 1f;

            foreach (TechnologySelectionTreeUI technologySelectionTree in technologySelectionTrees)
                technologySelectionTree.OnSelect -= TechnologySelectionTree_OnSelect;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
