using UnityEngine;

namespace Game
{
    public class TechnologyPanelUI : MonoBehaviour
    {
        public void Initialize(TechnologyTree technologyTree)
        {
            TechnologyPerkSlotUI[] technologyPerkSlotUIs = this.GetComponentsInChildren<TechnologyPerkSlotUI>();
            foreach (TechnologyPerkSlotUI technologyPerkSlotUI in technologyPerkSlotUIs)
            {
                technologyPerkSlotUI.Initialize(technologyTree);
            }

            TechnologyLevelRequirementUI[] technologyLevelRequirementUIs = this.GetComponentsInChildren<TechnologyLevelRequirementUI>();
            foreach (TechnologyLevelRequirementUI technologyLevelRequirementUI in technologyLevelRequirementUIs)
            {
                technologyLevelRequirementUI.Refresh(technologyTree.Agent);
            }
        }
    }
}
