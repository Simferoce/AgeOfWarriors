using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Technology
{
    [CreateAssetMenu(fileName = "TechnologyTreeDefinition", menuName = "Definition/Technology/TechnologyTreeDefinition")]
    public class TechnologyTreeDefinition : Definition
    {
        [Serializable]
        public class Row
        {
            [SerializeField] private List<TechnologyPerkDefinition> nodes;
            [SerializeField] private int level;

            public List<TechnologyPerkDefinition> Nodes { get => nodes; set => nodes = value; }
            public int Level { get => level; set => level = value; }
        }

        [SerializeField] private GameObject treeEditPanelPrefab;
        [SerializeField] private Sprite icon;
        [SerializeField] private List<Row> rows;

        public List<Row> Rows { get => rows; set => rows = value; }
        public GameObject TreeEditPanelPrefab { get => treeEditPanelPrefab; set => treeEditPanelPrefab = value; }
        public Sprite Icon { get => icon; set => icon = value; }

        public TechnologyTree Instantiate(Agent.AgentEntity agent)
        {
            return new TechnologyTree(agent, this);
        }
    }
}
