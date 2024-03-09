using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class DetectionCollision : MonoBehaviour
    {
        private List<GameObject> inCollisions = new List<GameObject>();

        public List<GameObject> InCollisions { get => inCollisions.Where(x => x != null && x.activeSelf).ToList(); }

        private void Update()
        {
            inCollisions = inCollisions.Where(x => x != null).ToList();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            inCollisions.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            inCollisions.Remove(collision.gameObject);
        }
    }
}
