using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Description
    {
        [SerializeField] private string text;
        [SerializeReference, SubclassSelector] private List<DescriptionParameter> parameters;

        public string Text { get => text; set => text = value; }

        public string Parse(Definition definition, object context)
        {
            if (parameters.Count == 0)
                return text;

            return string.Format(text, parameters.Select(x => x.GetValue(context)).ToArray());
        }
    }
}
