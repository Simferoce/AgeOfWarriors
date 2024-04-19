using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DealDamageOnShieldEndPerk", menuName = "Definition/Technology/Berserker/DealDamageOnShieldEndPerk")]
    public class DealDamageOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>, IAttackSource
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldableDestroyed += Shieldable_OnDestroyed;
                }

                modifiable.ModifierRemoved += Modifiable_ModifierRemoved;
            }

            private void Modifiable_ModifierRemoved(Game.Modifier obj)
            {
                if (obj is not ShieldModifierDefinition.Shield shield)
                    return;

                if (!modifiable.TryGetCachedComponent<Character>(out Character character))
                    return;

                float damage = Definition.GetValueOrThrow<float>(this, StatisticDefinition.Damage) * (shield.Initial - shield.Remaining);
                if (damage <= 0)
                    return;

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > Definition.GetValueOrThrow<float>(this, StatisticDefinition.Range))
                        continue;

                    attackable.TakeAttack(character.GenerateAttack(damage, 0, 0, attackable, this));
                }
            }

            private void Shieldable_OnDestroyed(IShieldable shieldable)
            {
                shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                modifiable.ModifierRemoved -= Modifiable_ModifierRemoved;
            }

            public override void Dispose()
            {
                base.Dispose();

                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                }

                modifiable.ModifierRemoved -= Modifiable_ModifierRemoved;
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
