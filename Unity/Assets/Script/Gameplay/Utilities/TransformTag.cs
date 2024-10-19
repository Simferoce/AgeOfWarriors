using UnityEngine;

namespace Game.Utilities
{
    public class TransformTag : MonoBehaviour
    {
        [SerializeField] private string id;

        public string Id { get => id; set => id = value; }
    }
}
