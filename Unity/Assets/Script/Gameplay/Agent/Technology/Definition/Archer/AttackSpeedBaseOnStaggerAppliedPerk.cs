using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedBaseOnStaggerAppliedPerk", menuName = "Definition/Technology/Archer/AttackSpeedBaseOnStaggerAppliedPerk")]
    public class AttackSpeedBaseOnStaggerAppliedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedBaseOnStaggerAppliedPerk>
        {
            private int amountOfStaggerApplied = 0;

            public override float? AttackSpeedPercentage => amountOfStaggerApplied * definition.attackSpeedByStaggerApplied;

            public Modifier(IModifiable modifiable, AttackSpeedBaseOnStaggerAppliedPerk modifierDefinition, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
                modifiable.GetCachedComponent<IModifierSource>().OnModifierAdded += Modifier_OnModifierAdded;
            }

            public override float? GetStack()
            {
                return amountOfStaggerApplied;
            }

            private void Modifier_OnModifierAdded(Game.Modifier modifier)
            {
                if (modifier is StaggerModifierDefinition.Modifier)
                    amountOfStaggerApplied++;
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<IModifierSource>().OnModifierAdded -= Modifier_OnModifierAdded;
            }
        }

        [SerializeField, Range(0, 5)] private float attackSpeedByStaggerApplied;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedByStaggerApplied);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
