using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDamageBaseOnEffectAppliedPerk", menuName = "Definition/Technology/Seer/IncreaseDamageBaseOnEffectAppliedPerk")]
    public class IncreaseDamageBaseOnEffectAppliedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDamageBaseOnEffectAppliedPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(IncreaseDamageBaseOnEffectAppliedPerk modifierDefinition) : base(modifierDefinition)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Update()
            {
                base.Update();
                attackPowerFlat.SetValue(definition.attackPowerPerEffectApplied * Source.AppliedModifiers.Count(x => x.Definition is not CharacterTechnologyPerkDefinition));
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float attackPowerPerEffectApplied;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerPerEffectApplied);
        }
    }
}
