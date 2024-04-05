using UnityEngine;

namespace Game
{
    public class OpenDetailAgentObject : MonoBehaviour
    {
        [SerializeField] private AgentObject agentObject;

        private void Awake()
        {
            GetComponentInParent<Canvas>().worldCamera = Camera.main;
        }

        public void OpenDetail()
        {
            DetailWindow.Open(agentObject);
        }
    }
}
