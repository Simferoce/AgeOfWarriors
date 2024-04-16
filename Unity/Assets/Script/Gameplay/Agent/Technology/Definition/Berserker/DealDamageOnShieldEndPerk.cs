using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DealDamageOnShieldEndPerk", menuName = "Definition/Technology/Berserker/DealDamageOnShieldEndPerk")]
    public class DealDamageOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>, IAttackSource
        {
            private float damagePerPointAbsorbed;
            private float percentageReach;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float damagePerPointRemaining, float percentageReach) : base(modifiable, modifierDefinition)
            {
                if (modifiable is IShieldable shieldable)
                {
                    shieldable.OnShieldableDestroyed += Shieldable_OnDestroyed;
                    shieldable.OnShieldBroken += Shieldable_OnShieldBroken;
                }

                this.damagePerPointAbsorbed = damagePerPointRemaining;
                this.percentageReach = percentageReach;
            }

            private void Shieldable_OnShieldBroken(Shield shield)
            {
                if (modifiable is not Character character)
                    return;

                float damage = damagePerPointAbsorbed * (shield.Initial - shield.Remaining);
                if (damage <= 0)
                    return;

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (Mathf.Abs((agent.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > character.Reach * percentageReach)
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

                if (modifiable is IShieldable shieldable)
                {
                    shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
                    shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                }
            }
        }

        [SerializeField] private float damagePerPointAbsorbed;
        [SerializeField, Range(0, 3)] private float percentageReach;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, damagePerPointAbsorbed, percentageReach);
        }
    }
}
