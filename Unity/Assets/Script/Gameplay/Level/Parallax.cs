using Game.Camera;
using UnityEngine;

namespace Game
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private new CameraBehaviour camera;
        [SerializeField] private float percentage = 1f;

        private Vector3 origin;

        private void Start()
        {
            origin = transform.position;
        }

        private void LateUpdate()
        {
            transform.position = new Vector3(origin.x + camera.Delta * percentage, transform.position.y, transform.position.z);
        }
    }
}