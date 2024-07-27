using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [CreateAssetMenu(fileName = "ColorRegistry", menuName = "Other/ColorRegistry")]
    public class ColorRegistry : ScriptableObject
    {
        public enum Identifiant
        {
            RequirementMet,
            RequirementNotMet
        }

        [Serializable]
        public class ColorPair
        {
            [SerializeField] private Identifiant identifiant;
            [SerializeField] private Color color;

            public Color Color { get => color; set => color = value; }
            public Identifiant Identifiant { get => identifiant; set => identifiant = value; }
        }

        [SerializeField] private List<ColorPair> colorPairs = new List<ColorPair>();

        public Color GetColor(Identifiant identifiant)
        {
            ColorPair colorPair = colorPairs.FirstOrDefault(x => x.Identifiant == identifiant);
            Assert.IsNotNull(colorPair, $"Could not find the color with the identifiant {identifiant} in the color registry.");

            return colorPair.Color;
        }
    }
}
