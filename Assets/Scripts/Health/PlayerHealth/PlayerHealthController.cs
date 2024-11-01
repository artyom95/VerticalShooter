using System;
using Settings;

namespace Health.PlayerHealth
{
    public class PlayerHealthController
    {
        public event Action PlayerHealthEndedEvent;
        public event Action<float> PlayerChangedEvent;
        private readonly PlayerHealthModel _playerHealthModel;
        private float _playerHealth;

        public PlayerHealthController(GameSettings gameSettings)
        {
            _playerHealth = gameSettings.PlayerHealth;
            _playerHealthModel = new PlayerHealthModel(_playerHealth);
        }
        
        public void DecreaseHealth()
        {
            var health = _playerHealthModel.DecreaseHealth();
            _playerHealth = health;
            PlayerChangedEvent?.Invoke(_playerHealth);
            CheckPlayerHealth();
        }

        private void CheckPlayerHealth()
        {
            if (_playerHealth <= 0)
            {
                PlayerHealthEndedEvent?.Invoke();
            }
        }
    }
}