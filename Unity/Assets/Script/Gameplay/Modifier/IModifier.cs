using System;

namespace Game
{
    public interface IModifier : IDisposable
    {
        float? SpeedPercentage { get => null; }
        float? AttackSpeedPercentage => null;
        float? ReachPercentage => null;
        float? Defense { get => null; }
        float? MaxHealth { get => null; }
        float? AttackPower { get => null; }
        bool? IsInvulnerable { get => null; }
    }
}
