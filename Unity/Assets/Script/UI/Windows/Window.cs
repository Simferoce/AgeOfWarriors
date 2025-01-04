using System;
using UnityEngine;

namespace Game.UI.Windows
{
    public abstract class Window : MonoBehaviour
    {
        public event Action<Window> OnShow;
        public event Action<Window> OnHide;

        public virtual bool IsUnique { get => true; }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke(this);

            if (!IsUnique)
                Destroy(gameObject);
        }
    }
}
