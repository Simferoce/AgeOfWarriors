using System.Collections.Generic;
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

        private AgentObject agentObject;

        public static DetailWindow Open(AgentObject agentObject)
        {
            DetailWindow statisticDetailWindow = WindowManager.Instance.GetWindow<DetailWindow>();
            statisticDetailWindow.Show();
            statisticDetailWindow.Refresh(agentObject);

            return statisticDetailWindow;
        }

        public void Refresh(AgentObject agentObject)
        {
            Time.timeScale = 0.0f;

            this.agentObject = agentObject;

            unitName.text = agentObject.GetDefinition().Title;
            unitIcon.sprite = agentObject.GetDefinition().Icon;

            healthImage.fillAmount = agentObject.Health / agentObject.MaxHealth.GetValue(null);
            healthText.text = agentObject.Health.ToString();
            maxHealthText.text = agentObject.MaxHealth.ToString();

            StatisticDetailUI[] statisticDetailUIs = GetComponentsInChildren<StatisticDetailUI>();

            foreach (StatisticDetailUI statisticDetailUI in statisticDetailUIs)
                statisticDetailUI.Refresh(agentObject);

            List<Modifier> modifiers = agentObject.GetModifiers();

            int i = 0;
            for (; i < modifiers.Count && i < modifierDetailUIs.Count; ++i)
            {
                modifierDetailUIs[i].gameObject.SetActive(true);
                modifierDetailUIs[i].Refresh(modifiers[i]);
            }

            for (; i < modifierDetailUIs.Count; ++i)
                modifierDetailUIs[i].gameObject.SetActive(false);

            if (agentObject as Character is Character character)
            {
                List<CharacterAbility> abilities = character.Abilities;
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
            else
            {
                for (int j = 0; j < abilityDetailUIs.Count; ++j)
                    abilityDetailUIs[j].gameObject.SetActive(false);
            }
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
