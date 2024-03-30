using System;

namespace Game
{
    [Serializable]
    public class PlayerBehaviour : AgentBehaviour
    {
        public override void OnLevelUp()
        {
            TechnologyWindow technologyWindow = WindowManager.Instance.GetWindow<TechnologyWindow>();
            //technologyWindow.Show();
        }
    }
}
