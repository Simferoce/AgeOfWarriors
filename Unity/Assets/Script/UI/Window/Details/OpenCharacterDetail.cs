using UnityEngine;

namespace Game
{
    public class OpenCharacterDetail : MonoBehaviour
    {
        [SerializeField] private Character character;

        private void Awake()
        {
            GetComponentInParent<Canvas>().worldCamera = Camera.main;
        }

        public void OpenDetail()
        {
            DetailWindow.Open(character);
        }
    }
}
