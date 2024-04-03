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

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;

        private TechnologyPerkDefinition technologyPerkDefinition;

        public static TechnologyDetailsPanelUI Open(TechnologyPerkDefinition technologyPerkDefinition)
        {
            TechnologyDetailsPanelUI technologyDetailsPanelUI = WindowManager.Instance.GetWindow<TechnologyDetailsPanelUI>();
            technologyDetailsPanelUI.icon.sprite = technologyPerkDefinition.Icon;
            technologyDetailsPanelUI.title.text = technologyPerkDefinition.Name;
            technologyDetailsPanelUI.description.text = "A Description";

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
