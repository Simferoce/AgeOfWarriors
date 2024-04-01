using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private Image image;

        public void Update()
        {
            image.fillAmount = Mathf.Clamp01(character.Health / character.MaxHealth);
        }
    }
}

