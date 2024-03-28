using System;

namespace Game
{
    [Serializable]
    public class Player : AgentBehaviour
    {
        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);

            agent.Technology.OnLevelUp += Technology_OnLevelUp;
        }

        private void Technology_OnLevelUp()
        {
            WindowManager.Instance.GetWindow<TechnologyWindow>().Show();
        }

        public override void Update()
        {

        }
    }
}
