using System;
using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [Serializable]
        public abstract class Instancier
        {
            public abstract Modifier Instantiate(IModifiable modifiable);
        }

        [Serializable]
        public abstract class Instancier<T> : Instancier
            where T : ModifierDefinition
        {
            [SerializeField] protected T definition;
        }

        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private bool showOnHealthBar = true;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public bool Show { get => showOnHealthBar; set => showOnHealthBar = value; }
        public string Description { get => description; set => description = value; }

        public virtual string ParseDescription()
        {
            return description;
        }
    }
}