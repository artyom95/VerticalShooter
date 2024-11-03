using Settings;
using UnityEngine;

namespace UI.Health
{
    public class PlayerHealthBarView

    {
        private HealthBar _healthBar;
        private float _maxHealth;
        private HealthBar _healthBarPrefab;
        private HealthBarFactory _healthBarFactory;
        private Transform _healthBarParent;

        public void Initialize(GameSettings gameSettings,
            HealthBar healthBarPrefab,
            HealthBarFactory healthBarFactory,
            Transform healthBarParent)
        {
            _healthBarParent = healthBarParent;
            _healthBarFactory = healthBarFactory;
            _healthBarPrefab = healthBarPrefab;
            _maxHealth = gameSettings.PlayerHealth;
        }

        public void CreateHealthBar()
        {
            _healthBar = _healthBarFactory.Create(_healthBarPrefab, _healthBarParent);
            _healthBar.Initialize(_maxHealth);
        }

        public void ShowHealth(float health)
        {
            _healthBar.SetHealthValue(health);
        }

        public void SetRoot(Player.Player player)
        {
            _healthBar.TryGetComponent<HealthBarPositionController>(out var healthBarPositionController);
            healthBarPositionController.SetPlayerRoot(player.gameObject);
        }

        public void DestroyHealthBar()
        {
            _healthBar.Destroy();
        }
    }
}