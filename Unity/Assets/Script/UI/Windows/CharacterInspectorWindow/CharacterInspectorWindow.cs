using Game.Ability;
using Game.Character;
using Game.Components;
using Game.Modifier;
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

        public static CharacterInspectorWindow Open(CharacterEntity character)
        {
            CharacterInspectorWindow statisticDetailWindow = WindowManager.Instance.GetWindow<CharacterInspectorWindow>();
            statisticDetailWindow.Show();
            statisticDetailWindow.Refresh(character);

            return statisticDetailWindow;
        }

        public void Refresh(CharacterEntity character)
        {
            Time.timeScale = 0.0f;

            unitName.text = character.GetDefinition().Title;
            unitIcon.sprite = character.GetDefinition().Icon;

            healthImage.fillAmount = character.Health / character.MaxHealth;
            healthText.text = character.Health.ToString();
            maxHealthText.text = character.MaxHealth.ToString();

            StatisticDetailUIElement[] statisticDetailUIs = GetComponentsInChildren<StatisticDetailUIElement>();

            foreach (StatisticDetailUIElement statisticDetailUI in statisticDetailUIs)
                statisticDetailUI.Refresh(character);

            List<ModifierEntity> modifiers = character.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsVisible).ToList();

            int i = 0;
            for (; i < modifiers.Count && i < modifierDetailUIs.Count; ++i)
            {
                modifierDetailUIs[i].gameObject.SetActive(true);
                modifierDetailUIs[i].Refresh(modifiers[i]);
            }

            for (; i < modifierDetailUIs.Count; ++i)
                modifierDetailUIs[i].gameObject.SetActive(false);

            List<AbilityEntity> abilities = character.GetCachedComponent<Caster>().Abilities.ToList();
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
            Time.timeScale = 1.0f;
        }

        public void Close()
        {
            Hide();
        }
    }
}
