using UnityEngine;

namespace Game
{
    public class Shield
    {
        public float Remaining { get; set; }
        public float Initial { get; set; }
        public float Duration { get; set; }

        public float startedAt;

        public Shield(float initial, float duration)
        {
            Initial = initial;
            Duration = duration;

            Remaining = Initial;
            startedAt = Time.time;
        }

        public bool Absorb(float amount, out float amountNotAbsorbed)
        {
            float absortion = Remaining - amount;
            Remaining -= Mathf.Min(Remaining, amount);
            amountNotAbsorbed = absortion < 0 ? -absortion : 0;

            return Remaining > 0;
        }

        public bool Update()
        {
            return Time.time - startedAt > Duration;
        }
    }
}
