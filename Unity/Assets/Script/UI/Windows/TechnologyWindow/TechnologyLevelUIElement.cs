using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class TechnologyLevelUIElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI;

        public void Refresh(Agent.AgentEntity agent)
        {
            textMeshProUGUI.text = agent.Technology.CurrentLevel.ToString();
        }
    }
}