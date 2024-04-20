using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private bool showOnHealthBar = true;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }
        public bool Show { get => showOnHealthBar; set => showOnHealthBar = value; }

        public string ParseDescription(object caller)
        {
            return ParseDescription(caller, this.description);
        }

        public virtual string ParseDescription(object caller, string description)
        {
            return this.description;
        }
    }
}
