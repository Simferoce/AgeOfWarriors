using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DealDamageOnShieldEndPerk", menuName = "Definition/Technology/Berserker/DealDamageOnShieldEndPerk")]
    public class DealDamageOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DealDamageOnShieldEndPerk>
        {
            public Modifier(ModifierHandler modifiable, DealDamageOnShieldEndPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.OnModifierAdded += Modifiable_ModifierRemoved;
            }

            private void Modifiable_ModifierRemoved(Game.Modifier obj)
            {
                if (obj is not ShieldModifierDefinition.Shield shield)
                    return;

                if (!modifiable.Entity.TryGetCachedComponent<Character>(out Character character))
                    return;

                float damage = definition.damagePerShieldPointAbsorbed * (shield.Initial - shield.Remaining);
                if (damage <= 0)
                    return;

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<Attackable>(out Attackable attackable))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReachExplosionRadius * character.Reach)
                        continue;

                    attackable.TakeAttack(agent.GetCachedComponent<AttackFactory>().Generate(damage, 0, 0, false, false, true, attackable));
                }
            }


            public override void Dispose()
            {
                base.Dispose();

                modifiable.OnModifierRemoved -= Modifiable_ModifierRemoved;
            }
        }

        [SerializeField] private float damagePerShieldPointAbsorbed;
        [SerializeField, Range(0, 5)] private float percentageReachExplosionRadius;

        public override string ParseDescription()
        {
            return string.Format(Description, damagePerShieldPointAbsorbed, StatisticFormatter.Percentage(percentageReachExplosionRadius, StatisticDefinition.Reach));
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
