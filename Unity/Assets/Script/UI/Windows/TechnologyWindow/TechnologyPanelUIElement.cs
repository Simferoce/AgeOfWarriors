using Game.Technology;
using UnityEngine;

namespace Game.UI.Windows
{
    public class UITechnologyPanelUIElement : MonoBehaviour
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
            TechnologyPerkSlotUIElement[] technologyPerkSlotUIs = GetComponentsInChildren<TechnologyPerkSlotUIElement>();
            foreach (TechnologyPerkSlotUIElement technologyPerkSlotUI in technologyPerkSlotUIs)
            {
                technologyPerkSlotUI.Initialize(technologyTree);
            }

            TechnologyLevelRequirementUIElement[] technologyLevelRequirementUIs = GetComponentsInChildren<TechnologyLevelRequirementUIElement>();
            foreach (TechnologyLevelRequirementUIElement technologyLevelRequirementUI in technologyLevelRequirementUIs)
            {
                technologyLevelRequirementUI.Refresh(technologyTree.Agent);
            }
        }
    }
}
