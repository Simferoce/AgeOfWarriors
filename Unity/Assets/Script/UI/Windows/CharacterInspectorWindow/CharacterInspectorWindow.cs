﻿using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CharacterInspectorWindow : Window
    {
        [SerializeField] private Image unitIcon;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI maxHealthText;
        [SerializeField] private List<ModifierDetailUIElement> modifierDetailUIs = new List<ModifierDetailUIElement>();
        [SerializeField] private List<AbilityDetailUIElement> abilityDetailUIs = new List<AbilityDetailUIElement>();

        public void Show(ICharacterInspectable character)
        {
            base.Show();
            Refresh(character);
        }

        public void Refresh(ICharacterInspectable character)
        {
            TimeManager.Instance.SetTimeScale(this, 0);

            unitName.text = character.GetTitle();
            unitIcon.sprite = character.GetIcon();

            float health = character.GetStatistic(StatisticDefinitionRegistry.Instance.Health);
            float maxHealth = character.GetStatistic(StatisticDefinitionRegistry.Instance.MaxHealth);
            healthImage.fillAmount = health / maxHealth;
            healthText.text = health.ToString();
            maxHealthText.text = maxHealth.ToString();

            StatisticDetailUIElement[] statisticDetailUIs = GetComponentsInChildren<StatisticDetailUIElement>();

            foreach (StatisticDetailUIElement statisticDetailUI in statisticDetailUIs)
                statisticDetailUI.Refresh(character);

            List<IModifierInspectable> modifiers = character.GetModifierInspectables().ToList();

            int i = 0;
            for (; i < modifiers.Count && i < modifierDetailUIs.Count; ++i)
            {
                modifierDetailUIs[i].gameObject.SetActive(true);
                modifierDetailUIs[i].Refresh(modifiers[i]);
            }

            for (; i < modifierDetailUIs.Count; ++i)
                modifierDetailUIs[i].gameObject.SetActive(false);

            List<IAbilityInspectable> abilities = character.GetAbilityInspectables();
            abilities.Reverse();

            int j = 0;
            for (; j < abilities.Count && j < abilityDetailUIs.Count; ++j)
            {
                abilityDetailUIs[j].gameObject.SetActive(true);
                abilityDetailUIs[j].Refresh(abilities[j]);
            }

            for (; j < abilityDetailUIs.Count; ++j)
                abilityDetailUIs[j].gameObject.SetActive(false);

        }

        public override void Hide()
        {
            base.Hide();
            TimeManager.Instance.ClearTimeScale(this);
        }

        public void Close()
        {
            Hide();
        }
    }
}
