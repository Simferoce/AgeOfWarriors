using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class UniqueModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private UniqueType type;

        public UniqueType Type { get => type; set => type = value; }
    }
}
