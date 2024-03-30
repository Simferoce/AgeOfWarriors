using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        public static Level Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Instance = null;
        }

        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private bool cheatCost;

        public bool CheatCost { get => cheatCost; }

        private void Awake()
        {
            Instance = this;
        }
    }
}