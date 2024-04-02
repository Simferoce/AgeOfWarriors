using UnityEngine;

namespace Game
{
    public abstract class TechnologyPerkDefinition : Definition
    {
        [SerializeField] private Sprite icon;

        public Sprite Icon { get => icon; set => icon = value; }
    }
}
