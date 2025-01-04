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

        public void Show(TechnologyTree technologyTree, TechnologyPerkDefinition technologyPerkDefinition)
        {
            base.Show();
            this.technologyTree = technologyTree;
            this.technologyPerkDefinition = technologyPerkDefinition;
            title.text = technologyPerkDefinition.Title;
            description.text = technologyPerkDefinition.ParseDescription(null);

            TechnologyPerkStatus technologyPerkStatus = technologyTree.GetStatus(technologyPerkDefinition);

            if (technologyPerkStatus is TechnologyPerkStatusUnlockable)
            {
                buttonImage.color = unlockableColor;
                button.interactable = true;
            }
            else if (technologyPerkStatus is TechnologyPerkStatusUnlocked)
            {
                buttonImage.color = unlockedColor;
                button.interactable = false;
            }
            else
            {
                buttonImage.color = lockedColor;
                button.interactable = false;
            }

            if (technologyPerkDefinition.HasRequirements())
            {
                requirements.text = $"REQUIRE - {technologyPerkDefinition.FormatRequirements(technologyTree)}";
            }
            else
            {
                requirements.text = "";
            }
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
