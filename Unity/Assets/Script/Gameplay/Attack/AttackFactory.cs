using System.Linq;
using UnityEngine;

namespace Game
{
    public class AttackFactory : MonoBehaviour, IComponent
    {
        public delegate void OnAttackLandedDelegate(AttackResult attackResult);
        public event OnAttackLandedDelegate OnAttackLanded;

        public Entity Entity { get; set; }

        public Attack Generate(Attackable target = null,
            float damage = 0f,
            float armorPenetration = 0f,
            float leach = 0f,
            Attack.Flag flags = Attack.Flag.None)
        {
            return new Attack(damage, armorPenetration, leach, flags, this);
        }

        public void NotifyAttackResult(AttackResult attackResult)
        {
            OnAttackLanded?.Invoke(attackResult);

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