using Game.UI.Windows;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ModalWindow : Window
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private ModalWindowChoiceUIElement choice1;
        [SerializeField] private ModalWindowChoiceUIElement choice2;

        public override bool IsUnique => false;

        public delegate void OnChoosenDelegate(int index);

        private List<string> choices;
        private OnChoosenDelegate callback;

        public void Show(string header, string description, List<string> choices, OnChoosenDelegate callback)
        {
            base.Show();
            this.choices = choices;
            this.header.text = header;
            this.description.text = description;
            this.callback = callback;

            choice1.Refresh(0, choices[0]);
            choice1.OnChosen += Choice2_OnChosen;

            if (choices.Count > 1)
            {
                choice2.gameObject.SetActive(true);
                choice2.Refresh(1, choices[1]);
                choice2.OnChosen += Choice2_OnChosen;
            }
            else
            {
                choice2.gameObject.SetActive(false);
            }

            TimeManager.Instance.SetTimeScale(this, 0f);
        }

        private void Choice2_OnChosen(ModalWindowChoiceUIElement modalWindowChoiceUIElement)
        {
            callback?.Invoke(modalWindowChoiceUIElement.Index);
            Hide();
        }

        public override void Hide()
        {
            base.Hide();
            choice1.OnChosen -= Choice2_OnChosen;
            choice2.OnChosen -= Choice2_OnChosen;

            TimeManager.Instance.ClearTimeScale(this);
        }

        public void Cancel()
        {
            callback?.Invoke(-1);
            Hide();
        }
    }
}
