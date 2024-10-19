using System;
using UnityEngine;

namespace Game.UI.Windows
{
    public class Window : MonoBehaviour
    {
        public event Action<Window> OnShow;
        public event Action<Window> OnHide;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke(this);
        }
    }
}
