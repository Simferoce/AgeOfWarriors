using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IStatisticContext
{
    private Dictionary<Type, List<IComponent>> cached = new Dictionary<Type, List<IComponent>>();

    private void Awake()
    {
        foreach (IComponent component in GetComponentsInChildren<IComponent>())
        {
            component.Entity = this;
        }
    }

    public bool TryGetCachedComponent<T>(out T component)
        where T : IComponent
    {
        component = GetCachedComponent<T>();
        return component != null;
    }

    public T GetCachedComponent<T>()
        where T : IComponent
    {
        if (cached.ContainsKey(typeof(T)))
        {
            return (T)cached[typeof(T)].First();
        }
        else
        {
            cached[typeof(T)] = GetComponentsInChildren<T>().Cast<IComponent>().ToList();
            return (T)cached[typeof(T)].First();
        }
    }

    public T AddOrGetCachedComponent<T>()
        where T : Component, IComponent
    {
        T component = GetCachedComponent<T>();
        if (component != null)
            return component;

        component = gameObject.AddComponent<T>();
        cached[typeof(T)] = new List<IComponent>() { component };

        return component;
    }

    public virtual bool IsName(ReadOnlySpan<char> name)
    {
        return name.SequenceEqual("entity");
    }

    public virtual IEnumerable<Statistic> GetStatistic()
    {
        yield break;
    }
}