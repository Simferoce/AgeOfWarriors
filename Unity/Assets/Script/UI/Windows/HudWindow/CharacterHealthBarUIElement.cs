using Game.Character;
using Game.Components;
using Game.Modifier;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CharacterHealthBarUIElement : MonoBehaviour
    {
        [SerializeField] private CharacterEntity character;
        [SerializeField] private Image image;
        [SerializeField] private Image shield;
        [SerializeField] private Image shieldUnderHealth;

        private List<HealthBarModifierUIElement> modifiersUI = new List<HealthBarModifierUIElement>();

        private void Awake()
        {
            modifiersUI = GetComponentsInChildren<HealthBarModifierUIElement>().ToList();
            foreach (HealthBarModifierUIElement modifier in modifiersUI)
                modifier.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (character.IsDead)
            {
                this.gameObject.SetActive(false);
                return;
            }

            if (Mathf.Sign(this.transform.lossyScale.x) == -1)
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            float shieldPercentage = character.GetCachedComponent<Attackable>().ShieldHandler.Remaining / character.MaxHealth;
            float healthPercentage = character.Health / character.MaxHealth;

            List<ModifierEntity> modifiers = character.GetCachedComponent<ModifierHandler>().GetModifiers().Where((ModifierEntity x) => x.IsVisible).ToList();

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

