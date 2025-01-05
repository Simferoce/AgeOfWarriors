using Game.Character;
using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CharacterDefintionInspectable : ICharacterInspectable
    {
        private CharacterDefinition characterDefinition;

        public CharacterDefintionInspectable(CharacterDefinition characterDefinition)
        {
            this.characterDefinition = characterDefinition;
        }

        public List<IAbilityInspectable> GetAbilityInspectables()
        {
            return characterDefinition.GetAbilities().Select(x => (IAbilityInspectable)new AbilityDefinitionInspectable(x)).ToList();
        }

        public Sprite GetIcon()
        {
            return characterDefinition.Icon;
        }

        public List<IModifierInspectable> GetModifierInspectables()
        {
            return new List<IModifierInspectable>();
        }

        public Statistic GetStatistic(StatisticDefinition definition)
        {
            if (definition == StatisticDefinitionRegistry.Instance.Health)
                return new StatisticFloat("health", StatisticDefinitionRegistry.Instance.Health, characterDefinition.GetStatistic(StatisticDefinitionRegistry.Instance.MaxHealth).Get<float>());

            return characterDefinition.GetStatistic(definition);
        }

        public string GetTitle()
        {
            return characterDefinition.Title;
        }
    }
}
