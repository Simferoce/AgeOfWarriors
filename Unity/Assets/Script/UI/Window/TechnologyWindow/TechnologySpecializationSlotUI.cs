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
            UnlockedB
        }

        [SerializeField] private TechnologyPerkDefinition choiceA;
        [SerializeField] private TechnologyPerkDefinition choiceB;

        [SerializeField] private Image iconA;
        [SerializeField] private Image iconB;
        [SerializeField] private Animator animator;

        private State state;

        private void OnEnable()
        {
            iconA.sprite = choiceA.Icon;
            iconB.sprite = choiceB.Icon;

            Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TechnologyChoicePanelUI technologyChoicePanelUI = TechnologyChoicePanelUI.Open(new List<TechnologyPerkDefinition>() { choiceA, choiceB });
            technologyChoicePanelUI.OnHidden += TechnologyChoicePanelUI_OnHidden;
        }

        private void TechnologyChoicePanelUI_OnHidden(Window window, Window.Result result)
        {
            window.OnHidden -= TechnologyChoicePanelUI_OnHidden;
            Refresh();
        }

        public void Refresh()
        {
            if (Agent.Player.Technology.PerksUnlocked.Contains(choiceA))
                SetState(State.UnlockedA);
            else if (Agent.Player.Technology.PerksUnlocked.Contains(choiceB))
                SetState(State.UnlockedB);
            else
                SetState(State.Unlockable);
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
                    animator.SetTrigger("UnlockedA");
                    break;
                case State.UnlockedB:
                    animator.SetTrigger("UnlockedB");
                    break;
            }
        }
    }
}
