using AgeOfWarriors.Core;
using System.Numerics;

namespace AgeOfWarriors
{
    public class Lane : Entity
    {
        private float length;

        public Lane(Game game, float length) : base(game)
        {
            this.length = length;
            AddComponent(new Transform(game, new Vector2(0, 0), Quaternion.Identity));
        }

        public Vector2 Snap(Vector2 position)
        {
            return new Vector2(position.X, GetComponent<Transform>().Position.Y);
        }

        public Vector2 GetMinimum()
        {
            Vector2 position = GetComponent<Transform>().Position;
            return new Vector2(position.X - length / 2, position.Y);
        }

        public Vector2 GetMaximum()
        {
            Vector2 position = GetComponent<Transform>().Position;
            return new Vector2(position.X + length / 2, position.Y);
        }
    }
}
