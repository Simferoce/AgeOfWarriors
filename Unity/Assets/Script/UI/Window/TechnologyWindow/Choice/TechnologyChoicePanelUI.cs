using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TechnologyChoicePanelUI : Window
    {
        [SerializeField] private GameObject template;
        [SerializeField] private RectTransform parent;

        private TechnologyDetailsPanelUI technologyDetailsPanelUI;

        public static TechnologyChoicePanelUI Open(List<TechnologyPerkDefinition> technologyPerkDefinitions)
        {
            TechnologyChoicePanelUI technologyChoicePanelUI = WindowManager.Instance.GetWindow<TechnologyChoicePanelUI>();
            for (int i = technologyChoicePanelUI.parent.childCount - 1; i >= 0; --i)
            {
                Destroy(technologyChoicePanelUI.parent.GetChild(i).gameObject);
            }

            foreach (TechnologyPerkDefinition technologyPerkDefinition in technologyPerkDefinitions)
            {
                GameObject gameObject = GameObject.Instantiate(technologyChoicePanelUI.template, technologyChoicePanelUI.parent);
                TechnologyChoicePerkUI technologyChoicePerkUI = gameObject.GetComponent<TechnologyChoicePerkUI>();

                technologyChoicePerkUI.gameObject.SetActive(true);
                technologyChoicePerkUI.Initialize(technologyPerkDefinition);
            }

            technologyChoicePanelUI.technologyDetailsPanelUI = WindowManager.Instance.GetWindow<TechnologyDetailsPanelUI>();
            technologyChoicePanelUI.technologyDetailsPanelUI.OnHidden += technologyChoicePanelUI.OnTechnologyDetailsPanelUIHidden;

            technologyChoicePanelUI.Show();
            return technologyChoicePanelUI;
        }

        private void OnTechnologyDetailsPanelUIHidden(Window window, Result result)
        {
            if (window is TechnologyDetailsPanelUI technologyDetailsPanelUI)
            {
                technologyDetailsPanelUI.OnHidden -= OnTechnologyDetailsPanelUIHidden;

                if (result is TechnologyDetailsPanelUI.DetailsResult detailsResult
                    && detailsResult.Value == TechnologyDetailsPanelUI.DetailsResult.ResultValue.Acquired)
                {
                    Hide(null);
                }
            }
        }

        public override void Hide(Result result)
        {
            base.Hide(result);

            if (technologyDetailsPanelUI)
                technologyDetailsPanelUI.OnHidden -= OnTechnologyDetailsPanelUIHidden;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
