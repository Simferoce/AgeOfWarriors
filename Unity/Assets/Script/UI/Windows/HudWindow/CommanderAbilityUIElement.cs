using Game.Components;
using Game.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CommanderAbilityUIElement : MonoBehaviour
    {
        [SerializeField] private Image fill;
        [SerializeField] private Image icon;
        [SerializeField] private int index;

        private Caster caster;

        public void Refresh(Caster caster)
        {
            this.caster = caster;

            if (caster.AbilityCount <= index)
            {
                gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = caster.Get(index).GetDefinition().Icon;
                gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            float cooldownPercentage = caster.Get(index).Remaining / caster.Get(index).Total;
            fill.fillAmount = 1 - Mathf.Clamp01(cooldownPercentage);

            fill.color = caster.Get(index).CanUse() ? WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red) : WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red) * WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
            icon.color = caster.Get(index).CanUse() ? Color.white : WindowManager.Instance.GetColor(ColorRegistry.Identifiant.LightGrayPurple);
        }

        public void OnClick()
        {
            if (caster.CanUse(index))
                caster.Use(index);
        }
    }
}
