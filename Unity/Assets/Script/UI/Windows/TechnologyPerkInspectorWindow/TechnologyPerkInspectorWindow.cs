using Game.Technology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class TechnologyPerkInspectorWindow : Window
    {
        public enum ResultValue
        {
            Acquired,
            Canceled
        }

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI requirements;
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockedColor;
        [SerializeField] private Color lockedColor;

        public ResultValue Result { get; private set; }

        private TechnologyPerkDefinition technologyPerkDefinition;
        private TechnologyTree technologyTree;

        public static TechnologyPerkInspectorWindow Open(TechnologyTree technologyTree, TechnologyPerkDefinition technologyPerkDefinition)
        {
            TechnologyPerkInspectorWindow technologyDetailsPanelUI = WindowManager.Instance.GetWindow<TechnologyPerkInspectorWindow>();
            technologyDetailsPanelUI.technologyTree = technologyTree;
            technologyDetailsPanelUI.technologyPerkDefinition = technologyPerkDefinition;
            technologyDetailsPanelUI.title.text = technologyPerkDefinition.Title;
            technologyDetailsPanelUI.description.text = technologyPerkDefinition.ParseDescription();

            TechnologyPerkStatus technologyPerkStatus = technologyTree.GetStatus(technologyPerkDefinition);

            if (technologyPerkStatus is TechnologyPerkStatusUnlockable)
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.unlockableColor;
                technologyDetailsPanelUI.button.interactable = true;
            }
            else if (technologyPerkStatus is TechnologyPerkStatusUnlocked)
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
                technologyDetailsPanelUI.requirements.text = $"REQUIRE - {technologyPerkDefinition.FormatRequirements(technologyTree)}";
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
            Result = ResultValue.Acquired;
            Hide();
        }

        public void Close()
        {
            Result = ResultValue.Canceled;
            Hide();
        }
    }
}
