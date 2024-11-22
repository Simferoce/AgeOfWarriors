
using System.Numerics;

namespace AgeOfWarriors
{
    public class Transform : Component
    {
        private Vector2 position;
        private Quaternion rotation;

        public Vector2 Position { get => position; set => position = value; }
        public Quaternion Rotation { get => rotation; set => rotation = value; }

        public Transform(Game game, Vector2 position, Quaternion rotation)
            : base(game)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void Translate(Vector2 translation)
        {
            position += translation;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}