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
            SetState(State.Unlockable);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Agent.Player.Technology.Acquire(technologyPerkDefinition);

            SetState(State.Unlocked);
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
    }
}
