using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedByBleedActivePerk", menuName = "Definition/Technology/Archer/AttackSpeedByBleedActivePerk")]
    public class AttackSpeedByBleedActivePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedByBleedActivePerk>
        {
            public override float? AttackSpeedPercentage => amountOfBleedApplied * definition.attackSpeedPerBleedApplied;
            public override bool Show => amountOfBleedApplied > 0;

            private int amountOfBleedApplied = 0;

            public Modifier(ModifierHandler modifiable, AttackSpeedByBleedActivePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }

            public override float? GetStack()
            {
                return amountOfBleedApplied;
            }

            public override void Update()
            {
                base.Update();

                amountOfBleedApplied = 0;
                foreach (Game.Modifier modifier in modifiable.Entity.GetCachedComponent<IModifierSource>().AppliedModifiers)
                {
                    if (modifier is not BleedingModifierDefinition.Modifier bleedingModifier)
                        continue;

                    amountOfBleedApplied += (int)modifier.GetStack();
                }
            }
        }

        [SerializeField, Range(0, 5)] private float attackSpeedPerBleedApplied;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedPerBleedApplied);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
