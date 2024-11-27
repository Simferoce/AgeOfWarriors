using UnityEngine;

namespace Game
{
    public abstract class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private string description;

        public string Title { get => title; }
        public string Description { get => description; set => description = value; }

        public virtual string ParseDescription()
        {
            return "";
        }

        public abstract Ability GetAbility();
    }
}
