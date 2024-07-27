using System;

namespace Game
{
    [Serializable]
    public class PlayerBehaviour : AgentBehaviour
    {
        public override void OnLevelUp()
        {
            TechnologyWindow.Open(Agent.Player);
        }
    }
}
