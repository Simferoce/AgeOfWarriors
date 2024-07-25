using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "TechnologyTreeDefinition", menuName = "Definition/Technology/TechnologyTreeDefinition")]
    public class TechnologyTreeDefinition : Definition
    {
        [Serializable]
        public class Row
        {
            [SerializeField] private List<TechnologyPerkDefinition> nodes;

            public List<TechnologyPerkDefinition> Nodes { get => nodes; set => nodes = value; }
        }

        [SerializeField] private List<Row> rows;

        public List<Row> Rows { get => rows; set => rows = value; }
    }
}
