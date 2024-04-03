using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyChoicePerkUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private TechnologyPerkDefinition technologyPerkDefinition;

        public void Initialize(TechnologyPerkDefinition technologyPerkDefinition)
        {
            this.technologyPerkDefinition = technologyPerkDefinition;
            icon.sprite = technologyPerkDefinition.Icon;
        }

        public void Choose()
        {
            TechnologyDetailsPanelUI.Open(technologyPerkDefinition);
        }
    }
}
