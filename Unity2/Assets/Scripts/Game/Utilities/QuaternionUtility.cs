using System.Numerics;

namespace AgeOfWarriors.Utilities
{
    public static class QuaternionUtility
    {
        public static Quaternion Left => Quaternion.CreateFromRotationMatrix(Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3(-1, 0, 0), new Vector3(0, 1, 0)));
        public static Quaternion Right => Quaternion.CreateFromRotationMatrix(Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3(1, 0, 0), new Vector3(0, 1, 0)));
    }
}
