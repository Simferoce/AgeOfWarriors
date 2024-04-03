using System;
using UnityEngine;

namespace Game
{
    public class Window : MonoBehaviour
    {
        public abstract class Result
        {

        }

        public event Action<Window> OnShowed;
        public event Action<Window, Result> OnHidden;

        public virtual void Show()
        {
            this.gameObject.SetActive(true);
            OnShowed?.Invoke(this);
        }

        public virtual void Hide(Result result)
        {
            this.gameObject.SetActive(false);
            OnHidden?.Invoke(this, result);
        }
    }
}
