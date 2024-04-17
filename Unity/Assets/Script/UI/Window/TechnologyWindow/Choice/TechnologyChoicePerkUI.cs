using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyChoicePerkUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;

        private TechnologyPerkDefinition technologyPerkDefinition;

        public void Initialize(TechnologyPerkDefinition technologyPerkDefinition)
        {
            this.technologyPerkDefinition = technologyPerkDefinition;
            icon.sprite = technologyPerkDefinition.Icon;
            title.text = technologyPerkDefinition.Title;
        }

        public void Choose()
        {
            TechnologyDetailsPanelUI.Open(technologyPerkDefinition);
        }
    }
}
