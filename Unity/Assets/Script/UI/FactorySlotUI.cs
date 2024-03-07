using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FactorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void Refresh(LaneObjectDefinition laneObjectDefinition)
        {
            if (laneObjectDefinition == null)
                icon.sprite = null;
            else
                icon.sprite = laneObjectDefinition.Icon;
        }
    }
}
