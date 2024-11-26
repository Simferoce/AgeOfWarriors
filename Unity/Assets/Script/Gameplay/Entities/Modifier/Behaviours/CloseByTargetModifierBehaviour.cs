using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class CloseByTargetModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        [SerializeReference, SubclassSelector] private RadiusModifierTarget target;

        public float CurrentStack { get => GetCloseByTargetCount(); set => throw new NotImplementedException(); }

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            target.Initialize(modifier);
        }

        private int GetCloseByTargetCount()
        {
            return target.GetTargets().Count;
        }
    }
}
