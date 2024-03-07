using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Lane lane;
        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private GameObject playerAgentPrefab;
        [SerializeField] private GameObject opponentAgentPrefab;
        [SerializeField] private Base playerBase;
        [SerializeField] private Base opponentBase;

        private Agent playerAgent;
        private Agent opponentAgent;

        private void Start()
        {
            playerAgent = GameObject.Instantiate(playerAgentPrefab).GetComponent<Agent>();
            opponentAgent = GameObject.Instantiate(opponentAgentPrefab).GetComponent<Agent>();

            playerBase.transform.position = lane.Ground.Project(playerBase.transform.position, out float positionPlayerBase);
            playerBase.Spawn(lane, playerAgent, 0, positionPlayerBase, 1);

            opponentBase.transform.position = lane.Ground.Project(opponentBase.transform.position, out float positionOppenentBase);
            opponentBase.Spawn(lane, opponentAgent, 0, positionOppenentBase, 1);

            playerAgent.SpawnLaneObject(lane, characterDefinition);
            playerAgent.SpawnLaneObject(lane, characterDefinition);
            //opponentAgent.SpawnLaneObject(lane, characterDefinition);
            //opponentAgent.SpawnLaneObject(lane, characterDefinition);
        }
    }
}