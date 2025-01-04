#if UNITY_EDITOR
using Game.Technology;
using Game.UI.Utilities;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class TechnologyPerkSlotUIElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;

        private TechnologyTree technologyTree;

        public void Initialize(TechnologyTree technologyTree)
        {
            this.technologyTree = technologyTree;

            technologyTree.Agent.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void OnDestroy()
        {
            if (technologyTree.Agent?.Technology != null)
                technologyTree.Agent.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TechnologyPerkInspectorWindow technologyPerkInspectorWindow = WindowManager.Instance.GetWindow<TechnologyPerkInspectorWindow>();
            technologyPerkInspectorWindow.Show(technologyTree, technologyPerkDefinition);
            technologyPerkInspectorWindow.OnHide += TechnologyDetailsPanelUI_OnHidden;
        }

        private void TechnologyDetailsPanelUI_OnHidden(Window window)
        {
            window.OnHide -= TechnologyDetailsPanelUI_OnHidden;
            Refresh();
        }

        public void Refresh()
        {
            if (technologyPerkDefinition == null)
                return;

            TechnologyPerkStatus technologyPerkStatus = technologyTree.GetStatus(technologyPerkDefinition);
            if (technologyPerkStatus is LockedTechnologyPerkStatus)
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
            else if (technologyPerkStatus is TechnologyPerkStatusUnlockable)
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red);
            else
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Yellow);

            icon.sprite = technologyPerkDefinition.TechnologyTreeIcon;
        }

        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, new GUIContent(technologyPerkDefinition != null ? technologyPerkDefinition.name : "Unassigned"));
        }
    }
}
