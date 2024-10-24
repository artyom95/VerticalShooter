using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour

    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill;
        private float _maxHealth;

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            SetHealthValue(maxHealth);
        }

        public void SetHealthValue(float currentHealth)
        {
            _healthBar.value = currentHealth / _maxHealth;
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            var color = _gradient.Evaluate(_healthBar.value);
            _fill.color = color;
        }
    }
}