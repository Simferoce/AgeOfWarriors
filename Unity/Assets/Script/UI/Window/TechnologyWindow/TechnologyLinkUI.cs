using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyLinkUI : MonoBehaviour
    {
        public enum State
        {
            Locked,
            Unlockable,
            Unlocked
        }

        [SerializeField] private Image visual;
        [SerializeField] private Color lockColor;
        [SerializeField] private Color unlockableColor;
        [SerializeField] private Color unlockColor;

        public void Refresh(State state)
        {
            switch (state)
            {
                case State.Locked:
                    visual.color = lockColor;
                    break;
                case State.Unlockable:
                    visual.color = unlockableColor;
                    break;
                case State.Unlocked:
                    visual.color = unlockColor;
                    break;
            }
        }
    }
}
