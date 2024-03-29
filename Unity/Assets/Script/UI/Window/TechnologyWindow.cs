using UnityEngine;

namespace Game
{
    public class TechnologyWindow : Window
    {
        public override void Show()
        {
            base.Show();
            Time.timeScale = 0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Hide();
        }

        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1f;
        }
    }
}
