using AgeOfWarriors.Core;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class EntityVisual : MonoBehaviour
    {
        private Entity entity;

        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public virtual void Refresh()
        {
            AgeOfWarriors.Core.Transform transform = entity.GetComponent<AgeOfWarriors.Core.Transform>();
            this.transform.position = new Vector3(transform.Position.X, transform.Position.Y, 0);
            this.transform.right = new Quaternion(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W) * Vector3.forward;
        }
    }
}
