using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform min;

    [SerializeField]
    private Transform max;

    private new Camera camera;
    private Vector3 lastMousePosition = Vector3.zero;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

            if (Mathf.Abs(deltaMousePosition.x) > 0.01f)
                this.transform.position += Vector3.right * speed * -deltaMousePosition.x * Time.deltaTime;

            float aspect = (float)Screen.width / Screen.height;

            float worldHeight = camera.orthographicSize * 2;

            float worldWidth = worldHeight * aspect;

            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, min.position.x + worldWidth / 2, max.position.x - worldWidth / 2), this.transform.position.y, this.transform.position.z);

            lastMousePosition = Input.mousePosition;
        }
    }
}
