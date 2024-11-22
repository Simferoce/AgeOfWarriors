namespace AgeOfWarriors
{
    public class Time
    {
        private float currentTime;
        private float deltaTime;

        public float CurrentTime { get => currentTime; }
        public float DeltaTime { get => deltaTime; }

        public void Update(float deltaTime)
        {
            currentTime += deltaTime;
            this.deltaTime = deltaTime;
        }
    }
}
