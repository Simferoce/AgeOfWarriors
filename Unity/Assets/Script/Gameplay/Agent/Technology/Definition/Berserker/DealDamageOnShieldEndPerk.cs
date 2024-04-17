using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DealDamageOnShieldEndPerk", menuName = "Definition/Technology/Berserker/DealDamageOnShieldEndPerk")]
    public class DealDamageOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>, IAttackSource
        {
            private StatisticReference<float> damagePerPointAbsorbed;
            private StatisticReference<float> distance;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, StatisticReference<float> damagePerPointRemaining, StatisticReference<float> distance) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldableDestroyed += Shieldable_OnDestroyed;
                    shieldable.OnShieldBroken += Shieldable_OnShieldBroken;
                }

                this.damagePerPointAbsorbed = damagePerPointRemaining;
                this.distance = distance;
            }

            private void Shieldable_OnShieldBroken(Shield shield)
            {
                if (!modifiable.TryGetCachedComponent<Character>(out Character character))
                    return;

                float damage = damagePerPointAbsorbed.GetValueOrThrow(this) * (shield.Initial - shield.Remaining);
                if (damage <= 0)
                    return;

                foreach (AgentObject agent in AgentObject.All.Select(x => x.GetCachedComponent<ITargeteable>()).Where(x => x != null))
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > character.Reach * distance.GetValueOrThrow(this))
                        continue;

                    Attack attack = new Attack(new AttackSource(new List<IAttackSource>() { character, this }), damage, 0, 0);
                    attackable.TakeAttack(attack);
                }
            }

            private void Shieldable_OnDestroyed(IShieldable shieldable)
            {
                shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
            }

            public override void Dispose()
            {
                base.Dispose();

                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
                    shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                }
            }
        }

        [SerializeField] private StatisticReference<float> damagePerPointAbsorbed;
        [SerializeField, Range(0, 3)] private StatisticReference<float> distance;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, damagePerPointAbsorbed, distance);
        }
    }
}
