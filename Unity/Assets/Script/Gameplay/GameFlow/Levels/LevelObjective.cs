using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class LevelObjective
    {
        [SerializeField] private string title;
        [SerializeField] private string description;

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
    }
}