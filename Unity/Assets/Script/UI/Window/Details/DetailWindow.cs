using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DetailWindow : Window
    {
        [SerializeField] private Image unitIcon;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI maxHealthText;
        [SerializeField] private List<ModifierDetailUI> modifierDetailUIs = new List<ModifierDetailUI>();
        [SerializeField] private List<AbilityDetailUI> abilityDetailUIs = new List<AbilityDetailUI>();

        public static DetailWindow Open(Character character)
        {
            DetailWindow statisticDetailWindow = WindowManager.Instance.GetWindow<DetailWindow>();
            statisticDetailWindow.Show();
            statisticDetailWindow.Refresh(character);

            return statisticDetailWindow;
        }

        public void Refresh(Character character)
        {
            Time.timeScale = 0.0f;

            unitName.text = character.GetDefinition().Title;
            unitIcon.sprite = character.GetDefinition().Icon;

            healthImage.fillAmount = character.Health / character.MaxHealth;
            healthText.text = character.Health.ToString();
            maxHealthText.text = character.MaxHealth.ToString();

            StatisticDetailUI[] statisticDetailUIs = GetComponentsInChildren<StatisticDetailUI>();

            foreach (StatisticDetailUI statisticDetailUI in statisticDetailUIs)
                statisticDetailUI.Refresh(character);

            List<Modifier> modifiers = character.GetCachedComponent<IModifiable>().GetModifiers();

            int i = 0;
            for (; i < modifiers.Count && i < modifierDetailUIs.Count; ++i)
            {
                modifierDetailUIs[i].gameObject.SetActive(true);
                modifierDetailUIs[i].Refresh(modifiers[i]);
            }

            for (; i < modifierDetailUIs.Count; ++i)
                modifierDetailUIs[i].gameObject.SetActive(false);

            List<Ability> abilities = character.Abilities.ToList();
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

        public override void Hide(Result result)
        {
            base.Hide(result);
            Time.timeScale = 1.0f;
        }

        public void Close()
        {
            this.Hide(null);
        }
    }
}
