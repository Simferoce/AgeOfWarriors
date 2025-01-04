using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class LevelTimerUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private void Update()
        {
            if (GameFlowManager.Instance.CurrentState is Level level)
            {
                float timeElapsed = level.TimeElapsed;
                int minutes = (int)(timeElapsed / 60);
                int seconds = (int)(timeElapsed % 60);
                timerText.text = $"{minutes:D2}:{seconds:D2}";
            }
        }
    }
}
