using Game;
using System;

[Serializable]
public class StatisticAbilityCasterProvider : StatisticAbilityProvider
{
    public override IStatisticContext Resolve(IStatisticContext context)
    {
        if (!(context is Ability ability))
            throw new ArgumentException();

        return ability.Caster;
    }
}

