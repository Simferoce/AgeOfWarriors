using UnityEngine;

namespace Game.UI.Windows
{
    public interface IModifierInspectable
    {
        public string GetTitle();
        public string GetDescription();
        public Sprite GetIcon();
        public float? GetPercentageRemainingDuration();
        public float? GetStack();
    }
}
