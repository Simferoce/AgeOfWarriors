﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class TechnologyPerkSlotUI : MonoBehaviour, IPointerClickHandler
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
            TechnologyDetailsPanelUI technologyDetailsPanelUI = TechnologyDetailsPanelUI.Open(technologyTree, technologyPerkDefinition);
            technologyDetailsPanelUI.OnHidden += TechnologyDetailsPanelUI_OnHidden;
        }

        private void TechnologyDetailsPanelUI_OnHidden(Window window, Window.Result result)
        {
            window.OnHidden -= TechnologyDetailsPanelUI_OnHidden;
            Refresh();
        }

        public void Refresh()
        {
            TechnologyHandler.TechnologyPerkStatus technologyPerkStatus = technologyTree.GetStatus(technologyPerkDefinition);
            if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusLocked)
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
            else if (technologyPerkStatus is TechnologyHandler.TechnologyPerkStatusUnlockable)
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red);
            else
                background.color = WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Yellow);

            icon.sprite = technologyPerkDefinition.TechnologyTreeIcon;
        }

        private void OnDrawGizmos()
        {
            Handles.Label(this.transform.position, new GUIContent(technologyPerkDefinition != null ? technologyPerkDefinition.name : "Unassigned"));
        }
    }
}
