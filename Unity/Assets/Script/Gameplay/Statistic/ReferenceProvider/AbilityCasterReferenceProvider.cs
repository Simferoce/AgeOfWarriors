using Game;
using System;

[Serializable]
public class AbilityCasterReferenceProvider : ReferenceProvider
{
    public override object Resolve(object context)
    {
        if (!(context is Ability ability))
            throw new ArgumentException();

        return ability.Caster;
    }
}

