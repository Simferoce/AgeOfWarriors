using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainAttackPowerOnAllyDiedPerk", menuName = "Definition/Technology/Berserker/GainAttackPowerOnAllyDiedPerk")]
    public class GainAttackPowerOnAllyDiedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainAttackPowerOnAllyDiedPerk>
        {
            public Modifier(GainAttackPowerOnAllyDiedPerk modifierDefinition) : base(modifierDefinition)
            {
                DeathEventChannel.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(DeathEventChannel.Event evt)
            {
                if (evt.AgentObject.Faction == (modifiable.Entity as AgentObject).Faction && evt.AgentObject != modifiable.Entity)
                {
                    Game.Modifier modifier = definition.Instantiate();
                    modifier.With(new CharacterModifierTimeElement(definition.buffDuration));

                    Source.Apply(modifiable, modifier,
                        new List<ModifierParameter>() { new ModifierParameter<float>("value", definition.attackPowerGain), new ModifierParameter<StatisticDefinition>("definition", StatisticDefinition.FlatAttackPower) });
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                DeathEventChannel.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private float attackPowerGain;
        [SerializeField] private float buffDuration;
        [SerializeField] private StatisticModifierDefinition attackPowerModifier;

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerGain, buffDuration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
