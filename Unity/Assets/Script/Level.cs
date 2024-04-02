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
        [SerializeField] private int maxCharacter = 10;
        [SerializeField] private float technologyGainMultiplier = 1;

        public bool CheatCost { get => cheatCost; }
        public int MaxCharacter { get => maxCharacter; set => maxCharacter = value; }
        public float TechnologyGainMultiplier { get => technologyGainMultiplier; }

        private void Awake()
        {
            Instance = this;
        }
    }
}