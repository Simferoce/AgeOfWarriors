using UnityEngine;

namespace Extension
{
    public static class TransformExtension
    {
        public static string GetFullPath(this Transform transform)
        {
            if (transform.parent == null)
                return transform.name;
            else
                return transform.parent.GetFullPath() + "/" + transform.name;
        }
    }
}
