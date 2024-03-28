using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseFormula", menuName = "Definition/Formula/Defense")]
    public class DefenseFormulaDefinition : FormulaDefinition
    {
        private static DefenseFormulaDefinition instance;
        public static DefenseFormulaDefinition Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<DefenseFormulaDefinition>("Definition/Formula/DefenseFormula");

                return instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            instance = null;
        }

        [SerializeField] private float growRate = 0.1f;

        public float ParseDamage(float damage, float defense)
        {
            return damage * (1 - (1 / (1 + defense * growRate)));
        }
    }
}
