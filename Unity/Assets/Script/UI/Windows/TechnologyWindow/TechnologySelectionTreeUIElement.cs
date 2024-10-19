using Game.Technology;
using Game.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class UITechnologySelectionTree : MonoBehaviour
    {
        [SerializeField] private Image image;

        public event System.Action<UITechnologySelectionTree> OnSelect;

        public TechnologyTree TechnologyTree { get => technologyTree; set => technologyTree = value; }

        private TechnologyTree technologyTree;

        public void Initialize(TechnologyTree technologyTree)
        {
            this.technologyTree = technologyTree;

            image.sprite = technologyTree.TechnologyTreeDefinition.Icon;
            Deselect();
        }

        public void Select()
        {
            image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.White);
            OnSelect?.Invoke(this);
        }

        public void Deselect()
        {
            image.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Gray);
        }
    }
}
