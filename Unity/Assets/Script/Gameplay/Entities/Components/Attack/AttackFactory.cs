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

        public Entity Entity { get; set; }
        public float LastTimeAttackLanded { get; private set; }

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public AttackData Generate(Attackable target = null,
            float damage = 0f,
            float armorPenetration = 0f,
            float leach = 0f,
            AttackData.Flag flags = AttackData.Flag.None)
        {
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

        public override string ToString()
        {
            return $"\"{string.Join("/", Entity.GetHierarchy().Reverse())}\"";
        }
    }
}