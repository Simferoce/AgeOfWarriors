using Game.UI.Windows;
using System.Collections;

namespace Game
{
    public class GameFlowManager : Manager<GameFlowManager>
    {
        public override IEnumerator InitializeAsync()
        {
            Universe.Instance.OnLoaded += Instance_OnLoaded;
            yield break;
        }

        private void Instance_OnLoaded(Universe universe)
        {
            Universe.Instance.OnLoaded -= Instance_OnLoaded;
            WindowManager.Instance.GetWindow<HudWindow>().Show();
        }
    }
}
