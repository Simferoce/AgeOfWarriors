using System.Linq;
using UnityEngine;

namespace Game.Components
{
    public class AttackFactory : MonoBehaviour
    {
        public delegate void OnAttackLandedDelegate(AttackResult attackResult);
        public event OnAttackLandedDelegate OnAttackLanded;

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
            return new AttackData(damage, armorPenetration, leach, flags, this);
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