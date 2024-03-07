using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        public static Level Instance;

        [SerializeField] private CharacterDefinition characterDefinition;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //playerAgent.SpawnLaneObject(lane, characterDefinition);
            //playerAgent.SpawnLaneObject(lane, characterDefinition);
            //opponentAgent.SpawnLaneObject(lane, characterDefinition);
            //opponentAgent.SpawnLaneObject(lane, characterDefinition);
        }
    }
}