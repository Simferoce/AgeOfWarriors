using UnityEngine;

namespace Game.Camera
{
    public class CameraBehaviour : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float minZoom;
        [SerializeField] private float maxZoom;
        [SerializeField, Range(0, 1)] private float ratioCenter = 0.2f;

        [Header("Drag")]
        [SerializeField] private float speed;

        [Header("Restriction")]
        [SerializeField] private Transform leftMax;
        [SerializeField] private Transform rightMax;
        [SerializeField] private Transform upMax;
        [SerializeField] private Transform downMax;

        private new UnityEngine.Camera camera;
        private Vector3 lastMousePosition = Vector3.zero;
        private Vector3 origin = Vector3.zero;

        public float Delta => camera.transform.position.x - origin.x;

        private void Awake()
        {
            camera = GetComponent<UnityEngine.Camera>();
            origin = camera.transform.position;
        }

        private void Update()
        {
            if (Time.timeScale <= 0)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
                Vector3 ratio = new Vector3(Screen.width / (camera.aspect * camera.orthographicSize), Screen.height / camera.orthographicSize, 1);
                Vector3 scaledMousePosition = Vector3.Scale(new Vector3(1f / (ratio.x / 2f), 1f / (ratio.y / 2f), 1), deltaMousePosition);

                if (camera.orthographicSize == maxZoom)
                    scaledMousePosition.y = 0f;

                this.transform.position += -scaledMousePosition;

                RestrictCamera();

                lastMousePosition = Input.mousePosition;
            }

            if (Input.mouseScrollDelta.y != 0)
                Zoom(-Input.mouseScrollDelta.y);
        }

        private void Zoom(float delta)
        {
            float deltaClamped = delta * zoomSpeed;
            deltaClamped = Mathf.Clamp(camera.orthographicSize + deltaClamped, minZoom, maxZoom) - camera.orthographicSize;

            //Zoom on the mouse
            Vector3 mouseViewport = camera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 convertedMouseViewport = new Vector3(mouseViewport.x * 2 - 1, mouseViewport.y * 2 - 1, 0);
            camera.transform.position += convertedMouseViewport * -deltaClamped;
            camera.orthographicSize += deltaClamped;

            RestrictCamera();
        }

        private void RestrictCamera()
        {
            float minX = leftMax.position.x + (camera.orthographicSize * camera.aspect);
            float maxX = rightMax.position.x - (camera.orthographicSize * camera.aspect);
            float minY = downMax.position.y + camera.orthographicSize;
            float maxY = upMax.position.y - camera.orthographicSize;

            camera.transform.position = new Vector3(
                Mathf.Clamp(camera.transform.position.x, minX, maxX),
                Mathf.Clamp(camera.transform.position.y, minY, maxY),
                camera.transform.position.z);
        }
    }
}