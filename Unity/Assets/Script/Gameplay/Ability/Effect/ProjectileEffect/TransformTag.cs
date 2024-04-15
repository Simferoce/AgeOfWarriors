using UnityEngine;

namespace Game
{
    public class TransformTag : MonoBehaviour
    {
        [SerializeField] private string id;

        public string Id { get => id; set => id = value; }
    }
}
