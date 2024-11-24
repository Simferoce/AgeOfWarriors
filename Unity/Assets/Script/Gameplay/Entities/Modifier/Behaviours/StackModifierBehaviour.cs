using System;

namespace Game.Modifier
{
    [Serializable]
    public class StackModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        private int currentStack = 0;

        public delegate void OnStackChangedDelegate(int oldValue, int newValue);
        public event OnStackChangedDelegate OnStackChanged;

        public int CurrentStack { get => currentStack; set => currentStack = value; }

        public override void Refresh()
        {
            IncreaseStack();
        }

        public void Clear()
        {
            currentStack = 0;
        }

        public void IncreaseStack()
        {
            int old = currentStack;
            currentStack++;
            OnStackChanged?.Invoke(old, currentStack);
        }

        public void DecreaseStack()
        {
            int old = currentStack;
            currentStack--;
            OnStackChanged?.Invoke(old, currentStack);
        }
    }
}
