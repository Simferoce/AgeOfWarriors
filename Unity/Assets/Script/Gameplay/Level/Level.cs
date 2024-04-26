using System.Globalization;
using System.Threading;
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

            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        [ContextMenu("LevelUp")]
        public void LevelUp()
        {
            Agent.Player.Technology.CurrentTechnology += Agent.Player.Technology.MaxTechnology;
        }
    }
}