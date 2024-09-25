using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedByBleedActivePerk", menuName = "Definition/Technology/Archer/AttackSpeedByBleedActivePerk")]
    public class AttackSpeedByBleedActivePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedByBleedActivePerk>
        {
            private StatisticModifiable<float> attackSpeedPerBleed = new StatisticModifiable<float>(definition: StatisticRepository.AttackSpeed);

            public override bool Show => amountOfBleedApplied > 0;

            private int amountOfBleedApplied = 0;

            public Modifier(ModifierHandler modifiable, AttackSpeedByBleedActivePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackSpeedPerBleed.Initialize(this);
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

                attackSpeedPerBleed.Modify(amountOfBleedApplied * definition.attackSpeedPerBleedApplied);
            }
        }

        [SerializeField] private StatisticSerialize<float> attackSpeedPerBleedApplied = new StatisticSerialize<float>("attack_speed_per_bleed", null, 1f);

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
