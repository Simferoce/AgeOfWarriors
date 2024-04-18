using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyPerkSlotUI : MonoBehaviour, IPointerClickHandler
    {
        public enum State
        {
            Locked,
            Unlockable,
            Unlocked
        }

        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;
        [SerializeField] private List<TechnologyLinkUI> links;
        [SerializeField] private Image icon;

        [SerializeField] private Color lockColor;
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockColor;

        private State state = State.Locked;

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

        public void SetState(State state)
        {
            this.state = state;

            switch (state)
            {
                case State.Locked:
                    icon.color = lockColor;
                    break;
                case State.Unlockable:
                    icon.color = unlockableColor;
                    break;
                case State.Unlocked:
                    icon.color = unlockColor;
                    break;
            }
        }

        public void Refresh()
        {
            if (technologyPerkDefinition.IsUnlocked(Agent.Player))
                SetState(State.Unlocked);
            else if (technologyPerkDefinition.IsUnlockable(Agent.Player))
                SetState(State.Unlockable);
            else
                SetState(State.Locked);

            for (int i = 0; i < links.Count; ++i)
            {
                bool hasRequirement = technologyPerkDefinition.RequirementsPerk[i].Execute(Agent.Player);
                bool hasPerk = technologyPerkDefinition.IsUnlocked(Agent.Player);
                if (hasRequirement && !hasPerk)
                {
                    links[i].Refresh(TechnologyLinkUI.State.Unlockable);
                }
                else if (hasRequirement && hasPerk)
                {
                    links[i].Refresh(TechnologyLinkUI.State.Unlocked);
                }
                else
                {
                    links[i].Refresh(TechnologyLinkUI.State.Locked);
                }
            }
        }
    }
}
