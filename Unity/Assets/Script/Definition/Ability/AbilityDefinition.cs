using UnityEngine;

namespace Game
{
    public abstract class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;

        public Sprite Icon { get => icon; set => icon = value; }
        public string Title { get => title; set => title = value; }

        public abstract CharacterAbility GetAbility();
    }
}
