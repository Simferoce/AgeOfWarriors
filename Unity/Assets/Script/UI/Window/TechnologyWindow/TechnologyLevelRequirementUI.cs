using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyLevelRequirementUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private int level;

        public void Refresh(Agent agent)
        {
            if (agent.Technology.CurrentLevel >= level)
                image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Yellow);
            else
                image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
        }
    }
}
