using UnityEngine;

namespace Game
{
    public class TechnologyWindow : Window
    {
        public static TechnologyWindow Open()
        {
            TechnologyWindow technologyWindow = WindowManager.Instance.GetWindow<TechnologyWindow>();
            technologyWindow.Show();

            Time.timeScale = 0f;

            return technologyWindow;
        }

        public override void Hide(Result result)
        {
            base.Hide(result);
            Time.timeScale = 1f;
        }

        public void Close()
        {
            Hide(null);
        }
    }
}
