using System;

[Serializable]
public abstract class ReferenceProvider
{
    public abstract object Resolve(object context);
}