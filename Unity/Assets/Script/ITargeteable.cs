using UnityEngine;

namespace Game
{
    public interface ITargeteable
    {
        public bool Attackable(GameObject from);
        public void Attack(float damage);
    }
}