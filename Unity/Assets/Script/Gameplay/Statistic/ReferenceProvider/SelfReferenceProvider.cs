using System;

[Serializable]
public class SelfReferenceProvider : ReferenceProvider
{
    public override object Resolve(object context)
    {
        return context;
    }
}