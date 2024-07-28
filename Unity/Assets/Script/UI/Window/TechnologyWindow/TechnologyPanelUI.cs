using UnityEngine;

namespace Game
{
    public class TechnologyPanelUI : MonoBehaviour
    {
        private TechnologyTree technologyTree;

        public void Initialize(TechnologyTree technologyTree)
        {
            this.technologyTree = technologyTree;
            technologyTree.OnPerkAcquired += TechnologyTree_OnPerkAcquired;

            Refresh();
        }

        private void TechnologyTree_OnPerkAcquired(TechnologyPerkDefinition obj)
        {
            Refresh();
        }

        private void OnDestroy()
        {
            technologyTree.OnPerkAcquired -= TechnologyTree_OnPerkAcquired;
        }

        public void Refresh()
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
