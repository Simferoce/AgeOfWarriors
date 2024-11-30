using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDamageTakenWhenStaggerPerk", menuName = "Definition/Technology/Archer/IncreaseDamageTakenWhenStaggerPerk")]
    public class IncreaseDamageTakenWhenStaggerPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDamageTakenWhenStaggerPerk>
        {
            private Statistic<float> damageTakenWhileStagger;

            public Modifier(IncreaseDamageTakenWhenStaggerPerk modifierDefinition) : base(modifierDefinition)
            {
                damageTakenWhileStagger = new Statistic<float>(StatisticDefinition.PercentageDamageTaken);
                StatisticRegistry.Register(damageTakenWhileStagger);
            }

            public override void Update()
            {
                base.Update();
                damageTakenWhileStagger.SetValue(modifiable.Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x.IsStagger == true) ? definition.increaseDamageTakenOfStaggered : 0f);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(damageTakenWhileStagger);
            }
        }

        [SerializeField, Range(0, 5)] private float increaseDamageTakenOfStaggered;

        public override string ParseDescription()
        {
            return string.Format(Description, increaseDamageTakenOfStaggered);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
