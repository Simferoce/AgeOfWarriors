using Game.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class TechnologyLevelRequirementUIElement : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private int level;

        public void Refresh(Agent.AgentEntity agent)
        {
            if (agent.Technology.CurrentLevel >= level)
                image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Yellow);
            else
                image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
        }
    }
}
