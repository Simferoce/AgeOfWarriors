using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class TechnologySpecializationSlotUI : MonoBehaviour, IPointerClickHandler
    {
        public enum State
        {
            Locked,
            Unlockable,
            UnlockedA,
            UnlockedB,
        }

        [SerializeField] private Image icon;
        [SerializeField] private Animator animator;
        [SerializeField] private ChoiceTechnologyDefinition choiceTechnologyPerkDefinition;
        [SerializeField] private List<TechnologyLinkUI> links;

        private State state;

        private void OnEnable()
        {
            animator.keepAnimatorControllerStateOnDisable = true;
            Agent.Player.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        private void OnDisable()
        {
            if (Agent.Player != null)
                Agent.Player.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TechnologyChoicePanelUI technologyChoicePanelUI = TechnologyChoicePanelUI.Open(choiceTechnologyPerkDefinition.Choices);
            technologyChoicePanelUI.OnHidden += TechnologyChoicePanelUI_OnHidden;
        }

        private void TechnologyChoicePanelUI_OnHidden(Window window, Window.Result result)
        {
            window.OnHidden -= TechnologyChoicePanelUI_OnHidden;
            Refresh();
        }

        public void Refresh()
        {
            if (Agent.Player.Technology.PerksUnlocked.Contains(choiceTechnologyPerkDefinition.Choices[0]))
                SetState(State.UnlockedA);
            else if (Agent.Player.Technology.PerksUnlocked.Contains(choiceTechnologyPerkDefinition.Choices[1]))
                SetState(State.UnlockedB);
            else if (choiceTechnologyPerkDefinition.IsUnlockable(Agent.Player))
                SetState(State.Unlockable);
            else
                SetState(State.Locked);

            for (int i = 0; i < links.Count; ++i)
            {
                bool hasRequirement = choiceTechnologyPerkDefinition.RequirementsPerk[i].Execute(Agent.Player);
                bool hasPerk = choiceTechnologyPerkDefinition.IsUnlocked(Agent.Player);
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

        public void SetState(State state)
        {
            this.state = state;

            switch (state)
            {
                case State.Locked:
                    animator.SetTrigger("Locked");
                    break;
                case State.Unlockable:
                    animator.SetTrigger("Unlockable");
                    break;
                case State.UnlockedA:
                    animator.SetTrigger("Unlock");
                    break;
                case State.UnlockedB:
                    animator.SetTrigger("Unlock");
                    break;
            }
        }
    }
}
