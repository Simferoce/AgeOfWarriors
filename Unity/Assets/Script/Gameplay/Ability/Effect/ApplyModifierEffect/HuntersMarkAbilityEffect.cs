using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HuntersMarkAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference duration;
        //[SerializeField] private HunterMarkModifierDefinition hunterMarkModifierDefinition;

        public override void Initialize(Ability ability)
        {
            base.Initialize(ability);
            damage.Initialize(ability);
            duration.Initialize(ability);
        }

        public override void Apply()
        {
            //ModifierHandler modifiable = Ability.Targets.Select(x => x.Entity.GetCachedComponent<ModifierHandler>()).Where(x => x != null).FirstOrDefault();

            //if (modifiable != null)
            //{
            //    HunterMarkModifierDefinition.Modifier huntersMark = (HunterMarkModifierDefinition.Modifier)modifiable.GetModifiers().FirstOrDefault(x => x is HunterMarkModifierDefinition.Modifier huntersMark && huntersMark.Source == this);

            //    if (huntersMark != null)
            //    {
            //        huntersMark.Refresh();
            //    }
            //    else
            //    {
            //        huntersMark = new HunterMarkModifierDefinition.Modifier(
            //                modifiable,
            //                hunterMarkModifierDefinition,
            //                this,
            //                damage,
            //                modifiable.Entity.GetCachedComponent<Attackable>(),
            //                Ability.Caster.Entity.GetCachedComponent<Character>())
            //            .With(new TimeModifierBehaviour(duration));

            //        modifiable.AddModifier(huntersMark);
            //    }
            //}
        }
    }
}
