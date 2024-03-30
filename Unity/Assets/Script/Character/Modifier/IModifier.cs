using System;

namespace Game
{
    public interface IModifier : IDisposable
    {
        float? SpeedPercentage { get; }
        float? Defense { get; }
    }
}
