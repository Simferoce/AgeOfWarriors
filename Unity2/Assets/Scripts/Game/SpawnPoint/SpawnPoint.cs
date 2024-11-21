using AgeOfWarriors.Core;
using System.Numerics;

namespace AgeOfWarriors
{
    public class SpawnPoint : Entity
    {
        public SpawnPoint(Game game, Vector2 position, Quaternion rotation) : base(game)
        {
            AddComponent(new Transform(game, position, rotation));
        }
    }
}
