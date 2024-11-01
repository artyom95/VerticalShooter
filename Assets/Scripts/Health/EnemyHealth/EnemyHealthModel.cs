using Interface;

namespace Health.EnemyHealth
{
    public class EnemyHealthModel: IHealthModel
    {
        private float _health;
        public EnemyHealthModel(float health)
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