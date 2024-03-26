using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform startPosition;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        Vector3 screenPosition = camera.WorldToViewportPoint(transform.position);
        if (screenPosition.x > 1.2)
        {
            transform.position = new Vector3(startPosition.position.x, transform.position.y, transform.position.z);
        }
    }
}
