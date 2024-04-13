using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private new CameraDrag camera;
    [SerializeField] private float percentage = 1f;

    private Vector3 origin;

    private void Start()
    {
        origin = transform.position;
    }

    private void LateUpdate()
    {
        this.transform.position = new Vector3(origin.x + camera.Delta * percentage, transform.position.y, transform.position.z);
    }
}
