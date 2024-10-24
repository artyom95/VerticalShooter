using System;
using Settings;

namespace Health.PlayerHealth
{
    public class PlayerHealthController
    {
        public event Action PlayerHealthEndedEvent;
        public event Action<float> PlayerChangedEvent;
        private readonly PlayerHealthModel _playerHealthModel;
        private  float _playerHealth;

        public PlayerHealthController( GameSettings gameSettings)
        {
            _playerHealthModel = new PlayerHealthModel();
            _playerHealth = gameSettings.PlayerHealth;
        }

        public void Initialize()
        {
            _playerHealthModel.Initialize(_playerHealth);
        }

        public void DecreaseHealth()
        {
            _playerHealth = _playerHealthModel.DecreaseHealth();
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