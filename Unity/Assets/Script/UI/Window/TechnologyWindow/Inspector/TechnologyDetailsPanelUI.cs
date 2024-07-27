using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyDetailsPanelUI : Window
    {
        public class DetailsResult : Result
        {
            public enum ResultValue
            {
                Acquired,
                Canceled
            }

            public ResultValue Value { get; set; }
        }

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI requirements;
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockedColor;
        [SerializeField] private Color lockedColor;

        private TechnologyPerkDefinition technologyPerkDefinition;
        private TechnologyTree technologyTree;

        public static TechnologyDetailsPanelUI Open(TechnologyTree technologyTree, TechnologyPerkDefinition technologyPerkDefinition)
        {
            TechnologyDetailsPanelUI technologyDetailsPanelUI = WindowManager.Instance.GetWindow<TechnologyDetailsPanelUI>();
            technologyDetailsPanelUI.technologyPerkDefinition = technologyPerkDefinition;
            technologyDetailsPanelUI.title.text = technologyPerkDefinition.Title;
            technologyDetailsPanelUI.description.text = technologyPerkDefinition.ParseDescription();

            TechnologyHandler.TechnologyPerkStatus technologyPerkStatus = technologyTree.GetStatus(technologyPerkDefinition);

            if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusUnlockable)
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.unlockableColor;
                technologyDetailsPanelUI.button.interactable = true;
            }
            else if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusUnlocked)
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.unlockedColor;
                technologyDetailsPanelUI.button.interactable = false;
            }
            else
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.lockedColor;
                technologyDetailsPanelUI.button.interactable = false;
            }

            if (technologyPerkDefinition.HasRequirements())
            {
                technologyDetailsPanelUI.requirements.text = $"REQUIRE - {technologyPerkDefinition.FormatRequirements(Agent.Player)}";
            }
            else
            {
                technologyDetailsPanelUI.requirements.text = "";
            }

            technologyDetailsPanelUI.Show();
            return technologyDetailsPanelUI;
        }

        public void Research()
        {
            technologyTree.Acquire(technologyPerkDefinition);
            Hide(new DetailsResult() { Value = DetailsResult.ResultValue.Acquired });
        }

        public void Close()
        {
            Hide(new DetailsResult() { Value = DetailsResult.ResultValue.Canceled });
        }
    }
}
