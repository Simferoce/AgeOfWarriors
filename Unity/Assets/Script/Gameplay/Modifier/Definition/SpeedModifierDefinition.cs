using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedModifierDefinition", menuName = "Definition/Modifier/SpeedModifierDefinition")]
    public class SpeedModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpeedModifierDefinition>
        {
            private StatisticModifiable<float> speedPercentage = new StatisticModifiable<float>(definition: StatisticRepository.SpeedPercentage);

            public Modifier(ModifierHandler modifiable, SpeedModifierDefinition modifierDefinition, float speed, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                speedPercentage.Initialize(this);
                speedPercentage.Modify(speed);
            }
        }
    }
}
