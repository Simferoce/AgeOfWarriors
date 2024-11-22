using AgeOfWarriors;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class EntityVisual : MonoBehaviour
    {
        private Entity entity;

        public Entity Entity { get => entity; set => entity = value; }

        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
        }

        public virtual void Refresh()
        {
            AgeOfWarriors.Transform transform = entity.GetComponent<AgeOfWarriors.Transform>();
            this.transform.position = new Vector3(transform.Position.X, transform.Position.Y, 0);
            this.transform.right = new Quaternion(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W) * Vector3.forward;
        }
    }
}
