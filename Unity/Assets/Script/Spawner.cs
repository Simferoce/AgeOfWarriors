using UnityEngine;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Agent agent;

        private void Start()
        {
            GameObject gameObject = GameObject.Instantiate(prefab, this.transform.position, this.transform.rotation);

            AgentObject agentObject = gameObject.GetComponent<AgentObject>();
            agent.Factory.SpawnAgentObject(agentObject);
        }
    }
}
