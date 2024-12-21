using Game.Technology;
using Game.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class UITechnologySelectionTree : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image background;

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
            background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red);
            OnSelect?.Invoke(this);
        }

        public void Deselect()
        {
            background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red) * WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Gray);
        }
    }
}
