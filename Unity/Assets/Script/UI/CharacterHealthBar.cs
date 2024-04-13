using System.Collections.Generic;
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

        private List<HealthBarModifierUI> modifiersUI = new List<HealthBarModifierUI>();

        private void Awake()
        {
            modifiersUI = GetComponentsInChildren<HealthBarModifierUI>().ToList();
            foreach (HealthBarModifierUI modifier in modifiersUI)
                modifier.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (Mathf.Sign(this.transform.lossyScale.x) == -1)
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            float shieldPercentage = character.Attackable.ShieldHandler.Shields.Sum(x => x.Remaining) / character.MaxHealth;
            float healthPercentage = character.Health / character.MaxHealth;

            List<Modifier> modifiers = character.GetModifiers();

            int i = 0;
            for (; i < modifiers.Count && i < modifiersUI.Count; ++i)
            {
                modifiersUI[i].gameObject.SetActive(true);
                modifiersUI[i].Refresh(modifiers[i]);
            }

            for (; i < modifiersUI.Count; ++i)
                modifiersUI[i].gameObject.SetActive(false);

            shieldUnderHealth.fillAmount = Mathf.Min(shieldPercentage, healthPercentage);
            shield.fillAmount = shieldPercentage;
            image.fillAmount = Mathf.Clamp01(healthPercentage);
        }
    }
}

