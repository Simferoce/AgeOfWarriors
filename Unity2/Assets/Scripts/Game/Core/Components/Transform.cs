
using System.Numerics;

namespace AgeOfWarriors.Core
{
    public class Transform : Component
    {
        private Vector3 position;
        private Quaternion rotation;

        public Vector3 Position { get => position; }

        public Transform(Game game, Vector3 position, Quaternion rotation)
            : base(game)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void Translate(Vector3 translation)
        {
            position += translation;
        }
    }
}