using AgeOfWarriors;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public abstract class VisualDefinition : ScriptableObject
    {
        public abstract bool IsVisualFor(Entity entity);
        public abstract EntityVisual Instantiate(Entity entity);
    }
}
