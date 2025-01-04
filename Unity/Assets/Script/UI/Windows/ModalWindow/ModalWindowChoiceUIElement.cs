using TMPro;
using UnityEngine;

namespace Game
{
    public class ModalWindowChoiceUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public delegate void OnChosenDelegate(ModalWindowChoiceUIElement modalWindowChoiceUIElement);
        public event OnChosenDelegate OnChosen;

        public int Index { get; private set; }

        public void Refresh(int index, string text)
        {
            this.text.text = text;
            this.Index = index;
        }

        public void OnClick()
        {
            OnChosen?.Invoke(this);
        }
    }
}
