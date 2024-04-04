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
        [SerializeField] private Image icon;
        [SerializeField] private Animator animator;

        private State state = State.Locked;

        private void OnEnable()
        {
            icon.sprite = technologyPerkDefinition.Icon;
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
                    break;
                case State.Unlockable:
                    animator.SetTrigger("Unlockable");
                    break;
                case State.Unlocked:
                    animator.SetTrigger("Unlock");
                    break;
            }
        }

        public void Refresh()
        {
            if (Agent.Player.Technology.PerksUnlocked.Contains(technologyPerkDefinition))
                SetState(State.Unlocked);
            else
                SetState(State.Unlockable);
        }
    }
}
