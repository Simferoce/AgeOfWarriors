using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDamageBaseOnEffectAppliedPerk", menuName = "Definition/Technology/Seer/IncreaseDamageBaseOnEffectAppliedPerk")]
    public class IncreaseDamageBaseOnEffectAppliedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDamageBaseOnEffectAppliedPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(ModifierHandler modifiable, IncreaseDamageBaseOnEffectAppliedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.AttackPowerFlat);
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

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerPerEffectApplied);
        }
    }
}
