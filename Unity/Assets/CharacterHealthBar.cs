using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private Image image;
        [SerializeField] private Image shield;
        [SerializeField] private Image shieldUnderHealth;

        public void Update()
        {
            if (Mathf.Sign(this.transform.localScale.x) == -1)
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            float shieldPercentage = character.Shields.Sum(x => x.Remaining) / character.MaxHealth.GetValue();
            float healthPercentage = character.Health / character.MaxHealth.GetValue();

            shieldUnderHealth.fillAmount = Mathf.Min(shieldPercentage, healthPercentage);
            shield.fillAmount = shieldPercentage;
            image.fillAmount = Mathf.Clamp01(healthPercentage);
        }
    }
}

