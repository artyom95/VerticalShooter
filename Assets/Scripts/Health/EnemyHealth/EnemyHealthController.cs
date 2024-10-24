using System;
using Settings;

namespace Health.EnemyHealth
{
    public class EnemyHealthController
    {
        public float AmountHealth { get; private set; }
        private Action _healthEndedAction;

        private Action<Enemy.Enemy,float> _onHealthChangedAction;

        private Enemy.Enemy _enemy;
        // public event Action<int> HealthChanged; 

        public void Initialize( Enemy.Enemy enemy,GameSettings gameSettings,
            Action healthEndedAction,
            Action<Enemy.Enemy, float> onHealthChangedAction)
        {
            _enemy = enemy;
            _onHealthChangedAction = onHealthChangedAction;
            AmountHealth = gameSettings.AmountEnemyHealth;
            _healthEndedAction = healthEndedAction;
        }

        public void DecreaseHealth()
        {
            AmountHealth -= 1;
           _onHealthChangedAction?.Invoke(_enemy, AmountHealth);
            CheckHealth();
        }

        private void CheckHealth()
        {
            if (AmountHealth <= 0)
            {
                _healthEndedAction?.Invoke();
            }
        }
    }
}