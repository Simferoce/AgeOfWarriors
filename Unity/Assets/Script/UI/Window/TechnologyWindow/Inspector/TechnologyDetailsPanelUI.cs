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
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockedColor;
        [SerializeField] private Color lockedColor;

        private TechnologyPerkDefinition technologyPerkDefinition;

        public static TechnologyDetailsPanelUI Open(TechnologyPerkDefinition technologyPerkDefinition)
        {
            TechnologyDetailsPanelUI technologyDetailsPanelUI = WindowManager.Instance.GetWindow<TechnologyDetailsPanelUI>();
            technologyDetailsPanelUI.technologyPerkDefinition = technologyPerkDefinition;
            technologyDetailsPanelUI.title.text = technologyPerkDefinition.Title;
            technologyDetailsPanelUI.description.text = technologyPerkDefinition.ParseDescription();

            bool isUnlockable = technologyPerkDefinition.IsUnlockable(Agent.Player);
            bool isUnlocked = technologyPerkDefinition.IsUnlocked(Agent.Player);
            if (isUnlockable && !isUnlocked)
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.unlockableColor;
                technologyDetailsPanelUI.button.interactable = true;
            }
            else if (isUnlocked)
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.unlockedColor;
                technologyDetailsPanelUI.button.interactable = false;
            }
            else
            {
                technologyDetailsPanelUI.buttonImage.color = technologyDetailsPanelUI.lockedColor;
                technologyDetailsPanelUI.button.interactable = false;
            }

            technologyDetailsPanelUI.Show();
            return technologyDetailsPanelUI;
        }

        public void Research()
        {
            Agent.Player.Technology.Acquire(technologyPerkDefinition);
            Hide(new DetailsResult() { Value = DetailsResult.ResultValue.Acquired });
        }

        public void Close()
        {
            Hide(new DetailsResult() { Value = DetailsResult.ResultValue.Canceled });
        }
    }
}
