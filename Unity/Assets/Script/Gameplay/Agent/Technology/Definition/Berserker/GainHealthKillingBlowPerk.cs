using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainHealthKillingBlowPerk", menuName = "Definition/Technology/Berserker/GainHealthKillingBlowPerk")]
    public class GainHealthKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainHealthKillingBlowPerk>
        {
            public Modifier(IModifiable modifiable, GainHealthKillingBlowPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded += AgentObject_OnAttackLanded;
            }

            private void AgentObject_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.KillingBlow && modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.Heal(definition.Heal(this));
            }

            public override void Dispose()
            {
                base.Dispose();
                if (modifiable.TryGetCachedComponent<Character>(out Character character))
                    character.OnAttackLanded -= AgentObject_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 1)] private float percentageHealMaxHealth;

        [Statistic("heal", nameof(HealFormat))] public float Heal(Modifier modifier) => (modifier.Modifiable as Character).MaxHealth * percentageHealMaxHealth;

        public string HealFormat(Modifier modifier) => StatisticFormatter.Percentage<Modifier>(Heal, percentageHealMaxHealth, StatisticDefinition.MaxHealth, modifier);

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
