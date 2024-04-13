using UnityEngine;

namespace Game
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private int direction = 1;

        public int Direction { get => direction; }

        public void Start()
        {
            Lane.Instance.Project(this.transform.position);
        }
    }
}
