using UnityEngine;

namespace Game
{
    public class SpawnPoint : MonoBehaviour
    {
        public int Direction { get; set; }

        public void Start()
        {
            this.transform.position = Lane.Instance.Project(this.transform.position);
        }
    }
}
