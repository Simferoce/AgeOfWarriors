using Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CharacterIconUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int index = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            Agent.Player.SpawnLaneObject(index);
        }
    }
}
