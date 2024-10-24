namespace Health.PlayerHealth
{
    public class PlayerHealthModel
    {
        private float _health;

        public void Initialize(float health)
        {
            _health = health;
        }

        public float DecreaseHealth()
        {
            _health -= 1;
            return _health;
        }
    }
}