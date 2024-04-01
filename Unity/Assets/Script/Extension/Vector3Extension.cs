using UnityEngine;

namespace Game
{
    public static class Vector3Extension
    {
        public static Vector3 XY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y);
        }
    }
}
