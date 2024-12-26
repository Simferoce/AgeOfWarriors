using Game.Agent;
using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnShieldEndDealDamageToNerbyEnemies : ModifierBehaviour
    {
        [SerializeField] private StatisticReference ratioDamage;
        [SerializeField] private StatisticReference range;

        private Attackable attackable;
        private AttackFactory attackFactory;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            attackable.ShieldHandler.OnShieldRemoved += OnShieldRemoved;
            attackFactory = modifier.AddOrGetCachedComponent<AttackFactory>();

            range.Initialize(modifier);
            ratioDamage.Initialize(modifier);
        }

        private void OnShieldRemoved(IShield shield)
        {
            AgentIdentity identity = modifier.Target.GetComponent<AgentIdentity>();

            foreach (Target target in Target.All)
            {
                if (!target.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity))
                    continue;

                if (identity.Faction == agentIdentity.Faction)
                    continue;

                if (Mathf.Abs(modifier.Target.Entity.transform.position.x - target.CenterPosition.x) > range.Get().Get<float>())
                    continue;

                if (!target.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                    continue;

                AttackData attack = attackFactory.Generate(
                                           target: attackable,
                                           damage: ratioDamage.Get().Get<float>() * (shield.InitialAmount - shield.Remaining)
                                           );

                attackable.TakeAttack(attack);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.ShieldHandler.OnShieldRemoved -= OnShieldRemoved;
        }
    }
}
