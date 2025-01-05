using Game.Character;
using Game.Components;
using Game.Modifier;
using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CharacterInstanceInspectable : ICharacterInspectable
    {
        private CharacterEntity character;

        public CharacterInstanceInspectable(CharacterEntity character)
        {
            this.character = character;
        }

        public List<IAbilityInspectable> GetAbilityInspectables()
        {
            return character.GetCachedComponent<Caster>().Abilities.Select(x => (IAbilityInspectable)new AbilityInstanceInspectable(x)).ToList();
        }

        public Sprite GetIcon()
        {
            return character.GetDefinition().Icon;
        }

        public List<IModifierInspectable> GetModifierInspectables()
        {
            return character.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsVisible).Select(x => (IModifierInspectable)new ModifierInstanceInspectable(x)).ToList();
        }

        public Statistic GetStatistic(StatisticDefinition definition)
        {
            return character[definition];
        }

        public string GetTitle()
        {
            return character.GetDefinition().Title;
        }
    }
}
