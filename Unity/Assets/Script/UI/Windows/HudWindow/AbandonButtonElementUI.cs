using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Windows
{
    public class AbandonButtonElementUI : MonoBehaviour
    {
        public void OnClick()
        {
            if (GameFlowManager.Instance.CurrentState is not Level level)
            {
                Debug.LogError($"Expecting the current state of the game to be of type \"{typeof(Level)}\" but it is \"{GameFlowManager.Instance.CurrentState.GetType()}\" isntead.");
                return;
            }

            ModalWindow modalWindow = WindowManager.Instance.GetWindow<ModalWindow>();
            modalWindow.Show("Abandon", "Do you want to abandon the current mission ?", new List<string>() { "Yes", "No" }, (int index) =>
            {
                if (index == 0)
                    level.Abandon();
            });
        }
    }
}
