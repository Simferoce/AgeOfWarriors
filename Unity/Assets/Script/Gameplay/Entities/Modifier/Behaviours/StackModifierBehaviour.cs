using System;

namespace Game.Modifier
{
    [Serializable]
    public class StackModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        private float currentStack = 0;

        public delegate void OnStackChangedDelegate(float oldValue, float newValue);
        public event OnStackChangedDelegate OnStackChanged;

        public float CurrentStack { get => currentStack; set => currentStack = value; }

        public override void Refresh()
        {
            IncreaseStack();
        }

        public void Clear()
        {
            currentStack = 0;
        }

        public void IncreaseStack(float amount = 1)
        {
            float old = currentStack;
            currentStack += amount;
            OnStackChanged?.Invoke(old, currentStack);
        }

        public void DecreaseStack(float amount = 1)
        {
            float old = currentStack;
            currentStack -= amount;
            if (currentStack < 0)
                currentStack = 0;

            OnStackChanged?.Invoke(old, currentStack);
        }

        public void SetStack(float amount)
        {
            float old = currentStack;
            currentStack = amount;

            OnStackChanged?.Invoke(old, currentStack);
        }
    }
}
