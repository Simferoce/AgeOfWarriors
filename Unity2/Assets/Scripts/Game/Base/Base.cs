using AgeOfWarriors;
using System.Numerics;

namespace AgeOfWarriors
{
    public class Base : Entity
    {
        public SpawnPoint SpawnPoint { get; private set; }

        public Base(Game game, Vector2 position, Quaternion rotation) : base(game)
        {
            AddComponent(new Transform(game, position, rotation));
            SpawnPoint = new SpawnPoint(game, position, rotation);
        }
    }
}
