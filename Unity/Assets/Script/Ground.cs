using UnityEngine;

namespace Game
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private float bounds;

        private float TotalLength => bounds * 2;

        public float Clamp(float length)
        {
            return Mathf.Clamp(length, 0, TotalLength);
        }

        public Vector3 GetPosition(float length)
        {
            length = Clamp(length);

            return this.transform.position + (Vector3.right * (length - TotalLength / 2));
        }

        public Vector3 Project(Vector3 point, out float length)
        {
            length = Clamp(bounds + point.x - this.transform.position.x);
            return new Vector3(GetPosition(length).x, this.transform.position.y, point.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(this.transform.position + Vector3.right * bounds, 0.1f);
            Gizmos.DrawSphere(this.transform.position - Vector3.right * bounds, 0.1f);
            Gizmos.DrawLine(this.transform.position + Vector3.right * bounds, this.transform.position - Vector3.right * bounds);
        }
    }
}