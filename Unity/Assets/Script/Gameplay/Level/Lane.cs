using UnityEngine;

namespace Game
{
    public class Lane : MonoBehaviour
    {
        public static Lane Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        public Vector3 Project(Vector3 point)
        {
            return new Vector3(point.x, this.transform.position.y, point.z);
        }
    }
}