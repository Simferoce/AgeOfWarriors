﻿using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    public class ModifierHandler : MonoBehaviour
    {
        public event Action<ModifierEntity> OnModifierRemoved;
        public event Action<ModifierEntity> OnModifierAdded;
        public Entity Entity { get; set; }

        private List<ModifierEntity> modifiers = new List<ModifierEntity>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public void Add(ModifierEntity modifier)
        {
            Entity.GetCachedComponent<StatisticIndex>().Add(modifier.GetCachedComponent<StatisticIndex>());
            modifiers.Add(modifier);
            OnModifierAdded?.Invoke(modifier);
        }

        public void Remove(ModifierEntity modifier)
        {
            Entity.GetCachedComponent<StatisticIndex>().Remove(modifier.GetCachedComponent<StatisticIndex>());
            modifiers.Remove(modifier);
            OnModifierRemoved?.Invoke(modifier);
        }

        public List<ModifierEntity> GetModifiers()
        {
            return modifiers;
        }

        public bool TryGetModifier(ModifierDefinition definition, out ModifierEntity modifier)
        {
            modifier = modifiers.FirstOrDefault(x => x.Definition == definition);
            return modifier != null;
        }
    }
}