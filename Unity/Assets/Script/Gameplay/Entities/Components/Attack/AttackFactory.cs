using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Components
{
    public class AttackFactory : MonoBehaviour
    {
        public delegate void OnAttackLandedDelegate(AttackResult attackResult);
        public event OnAttackLandedDelegate OnAttackLanded;

        public delegate void OnGenerateAttackDelegate(AttackData attackData);
        public event OnGenerateAttackDelegate OnGenerateAttack;

        public delegate void OnDestroyDelegate(AttackFactory attackFactory);
        public event OnDestroyDelegate OnDeactivated;

        public delegate void OnTakeDownDelegate(AttackResult result);
        public event OnTakeDownDelegate OnTakeDown;

        public Entity Entity { get; set; }
        public float LastTimeAttackLanded { get; private set; }

        private List<Attackable> attackedAttackables = new List<Attackable>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
            Entity.OnDeactivated += Entity_OnDeactivated;
            LastTimeAttackLanded = float.MinValue;
        }

        private void Entity_OnDeactivated(Entity entity)
        {
            OnDeactivated?.Invoke(this);
        }

        private void OnDestroy()
        {
            Entity.OnDeactivated -= Entity_OnDeactivated;
        }

        public AttackData Generate(Attackable target = null,
            float damage = 0f,
            float armorPenetration = 0f,
            float leach = 0f,
            AttackData.Flag flags = AttackData.Flag.None)
        {
            if (target != null && target.Entity.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.Weak, out Statistic statisticWeak) && statisticWeak.Get<bool>())
                damage += Entity.StatisticRepository.TryGet<float>(StatisticDefinitionRegistry.Instance.FlatDamageVersusWeak, out Statistic<float> flatDamageVersusWeak) ? flatDamageVersusWeak : 0f;

            AttackData attackData = new AttackData(damage, armorPenetration, leach, flags, this);
            NotifyGenerateAttack(attackData);
            return attackData;
        }

        public void NotifyGenerateAttack(AttackData attackData)
        {
            OnGenerateAttack?.Invoke(attackData);

            foreach (Entity entity in Entity.GetHierarchy())
            {
                if (entity == Entity)
                    continue;

                if (entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                {
                    attackFactory.NotifyGenerateAttack(attackData);
                    return;
                }
            }
        }

        public void NotifyAttackResult(AttackResult attackResult)
        {
            if (!attackedAttackables.Contains(attackResult.Target))
            {
                attackedAttackables.Add(attackResult.Target);
                attackResult.Target.OnDeactivated += AttackResultTargetOnDeactivated;
                attackResult.Target.OnDamageTaken += Target_OnDamageTaken;
            }

            OnAttackLanded?.Invoke(attackResult);
            LastTimeAttackLanded = Time.time;

            foreach (Entity entity in Entity.GetHierarchy())
            {
                if (entity == Entity)
                    continue;

                if (entity.TryGetCachedComponent<AttackFactory>(out AttackFactory attackFactory))
                {
                    attackFactory.NotifyAttackResult(attackResult);
                    return;
                }
            }
        }

        private void Target_OnDamageTaken(AttackResult result, Attackable receiver)
        {
            if (result.KillingBlow)
                OnTakeDown?.Invoke(result);
        }

        private void AttackResultTargetOnDeactivated(Attackable attackable)
        {
            attackable.OnDeactivated -= AttackResultTargetOnDeactivated;
            attackable.OnDamageTaken -= Target_OnDamageTaken;
            attackedAttackables.Add(attackable);
        }

        public override string ToString()
        {
            return $"\"{string.Join("/", Entity.GetHierarchy().Reverse())}\"";
        }
    }
}