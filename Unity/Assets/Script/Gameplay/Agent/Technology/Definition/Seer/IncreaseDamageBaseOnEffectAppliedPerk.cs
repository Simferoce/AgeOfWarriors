using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDamageBaseOnEffectAppliedPerk", menuName = "Definition/Technology/Seer/IncreaseDamageBaseOnEffectAppliedPerk")]
    public class IncreaseDamageBaseOnEffectAppliedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDamageBaseOnEffectAppliedPerk>
        {
            public override float? AttackPower => definition.attackPowerPerEffectApplied * Source.AppliedModifiers.Count(x =>
                x.Definition is not CharacterTechnologyPerkDefinition
                && x.Modifiable.TryGetCachedComponent<ITargeteable>(out ITargeteable target)
                && Source.TryGetCachedComponent<ITargeteable>(out ITargeteable sourceTarget)
                && target.Faction != sourceTarget.Faction);

            public Modifier(IModifiable modifiable, IncreaseDamageBaseOnEffectAppliedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField] private float attackPowerPerEffectApplied;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerPerEffectApplied);
        }
    }
}
