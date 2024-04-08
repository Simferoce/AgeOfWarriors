using UnityEngine;

namespace Game
{
    public abstract class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;

        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat range;
        [StatisticResolve("range")] public IStatisticFloat Range => range;

        public Sprite Icon { get => icon; set => icon = value; }
        public string Title { get => title; set => title = value; }

        public abstract CharacterAbility GetAbility();
    }
}
