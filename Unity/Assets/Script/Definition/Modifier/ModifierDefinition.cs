using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;

        public Sprite Icon { get => icon; }
        public string Title { get => title; set => title = value; }
    }
}
