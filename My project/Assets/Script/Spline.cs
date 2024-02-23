using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spline : MonoBehaviour
{
    [SerializeField] private List<Transform> points;

    private float totalLength = 0f;

    private void Awake()
    {
        for (int i = 1; i < points.Count; ++i)
        {
            totalLength += Vector3.Distance(points[i - 1].position, points[i].position);
        }
    }


    public float Clamp(float length)
    {
        return Mathf.Clamp(length, 0, totalLength);
    }

    public Vector3 GetPosition(float length)
    {
        length = Clamp(length);

        float currentLength = 0f;
        for (int i = 1; i < points.Count; ++i)
        {
            float distance = Vector3.Distance(points[i - 1].position, points[i].position);
            if(currentLength + distance > length)
            {
                float relativeLength = currentLength + distance - length;
                float normalizedLength = relativeLength / distance;

                return Vector3.Lerp(points[i - 1].position, points[i].position, 1 - normalizedLength);
            }

            currentLength += distance;
        }

        return points[^1].position;
    }

    public Vector3 Project(Vector3 point, out float length)
    {
        (float distance, Vector3 point, float length) result = (float.MaxValue, point, 0);

        float currentLength = 0f;
        for (int i = 1; i < points.Count; ++i)
        {
            Vector3 line = points[i].position - points[i - 1].position;
            Vector3 linePoint = point - points[i - 1].position;
            if (Vector3.Dot(linePoint, line) < 0)
                continue;

            Vector3 projection = points[i - 1].position + Vector3.Project(linePoint, line);

            float distance = Vector3.Distance(projection, point);
            float projectLength = Vector3.Distance(projection, points[i - 1].position);

            if (distance < result.distance && projectLength < line.magnitude)
            {
                result = (distance, projection, projectLength + currentLength);
            }

            currentLength += line.magnitude;
        }

        length = result.length;
        return result.point;
    }
}

