using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpeedModifierDefinition", menuName = "Definition/Modifier/SpeedModifierDefinition")]
    public class SpeedModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpeedModifierDefinition>
        {
            private Statistic<float> speedPercentage;

            public Modifier(ModifierHandler modifiable, SpeedModifierDefinition modifierDefinition, float speed, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                speedPercentage = new Statistic<float>(StatisticDefinition.SpeedPercentage, speed);
                StatisticRegistry.Register(speedPercentage);
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, Mathf.Abs(speedPercentage.GetValue()));
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(speedPercentage);
            }
        }
    }
}
