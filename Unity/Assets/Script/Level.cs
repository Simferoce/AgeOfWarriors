using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        public static Level Instance;

        [SerializeField] private CharacterDefinition characterDefinition;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Agent.Opponent.SpawnLaneObject(characterDefinition);
        }
    }
}