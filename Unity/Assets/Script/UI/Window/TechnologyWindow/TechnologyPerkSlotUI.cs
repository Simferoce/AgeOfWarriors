#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyPerkSlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;
        [SerializeField] private Image icon;

        [SerializeField] private Color lockColor;
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockColor;

        private void OnEnable()
        {
            Agent.Player.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void OnDisable()
        {
            if (Agent.Player != null)
                Agent.Player.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TechnologyDetailsPanelUI technologyDetailsPanelUI = TechnologyDetailsPanelUI.Open(technologyPerkDefinition);
            technologyDetailsPanelUI.OnHidden += TechnologyDetailsPanelUI_OnHidden;
        }

        private void TechnologyDetailsPanelUI_OnHidden(Window window, Window.Result result)
        {
            window.OnHidden -= TechnologyDetailsPanelUI_OnHidden;
            Refresh();
        }

        public void Refresh()
        {
            TechnologyHandler.TechnologyPerkStatus technologyPerkStatus = Agent.Player.Technology.GetStatus(technologyPerkDefinition);
            if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusLocked)
                icon.color = lockColor;
            else if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusUnlockable)
                icon.color = unlockableColor;
            else
                icon.color = unlockColor;
        }

        private void OnDrawGizmos()
        {
            Handles.Label(this.transform.position, new GUIContent(technologyPerkDefinition != null ? technologyPerkDefinition.name : "Unassigned"));
        }
    }
}
