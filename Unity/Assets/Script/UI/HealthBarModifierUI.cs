using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarModifierUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void Refresh(Modifier modifier)
        {
            icon.sprite = modifier.Definition.Icon;
        }
    }
}
