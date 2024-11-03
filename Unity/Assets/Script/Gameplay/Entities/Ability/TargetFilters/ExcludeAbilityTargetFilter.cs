using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class ExcludeAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeField] private List<Entity.EntityTag> types = new List<Entity.EntityTag>();

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return targetEntity.Tags.All(x => !types.Contains(x));
        }
    }
}