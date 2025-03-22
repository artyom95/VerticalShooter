using System;
using Health.EnemyHealth;
using Interfaces;
using Settings;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, IMovable
    {
        public bool IsAlive { get; private set; }
        public event Action<Enemy> ReleaseEnemyAction;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Action<Enemy> _enemyDestroyedAction;
        private Action<Enemy> _deleteEnemyFromList;
        private Action<Enemy> _enemyDeathAction;

        private EnemyHealthController _enemyHealthController;
        private float _enemySpeed;
        private bool _startMoving;
        private Color _defaultColor;
        private GameSettings _gameSettings;
        private Action<Enemy, float> _onHealthChangedAction;

        public void Start()
        {
            IsAlive = true;
            _defaultColor = _spriteRenderer.color;
        }

        public void Initialize(GameSettings gameSettings, Action<Enemy> enemyDestroyedAction,
            Action<Enemy> deleteEnemyFromList, Action<Enemy> enemyDeathAction,
            Action<Enemy, float> onHealthChangedAction)
        {
            _onHealthChangedAction = onHealthChangedAction;
            _gameSettings = gameSettings;
            gameObject.transform.Rotate(0, 0, 180);
            _enemyDeathAction = enemyDeathAction;
            _deleteEnemyFromList = deleteEnemyFromList;
            _enemyDestroyedAction = enemyDestroyedAction;
            _enemyHealthController = new EnemyHealthController();
            _enemySpeed = gameSettings.EnemySpeed;
            _enemyHealthController.Initialize(this, gameSettings, KillEnemy, onHealthChangedAction);
        }

        public void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.TryGetComponent<Bullet>(out var bullet))
            {
                bullet.ReleaseBullet();
                _enemyHealthController.DecreaseHealth();
            }

            if (_enemyHealthController.AmountHealth <= 0)
            {
                gameObject.TryGetComponent<Enemy>(out var enemy);
                _enemyDeathAction?.Invoke(enemy);
                KillEnemy();
            }
        }

        public void KillEnemy()
        {
            if (gameObject != null && gameObject.activeSelf)
            {
                _deleteEnemyFromList?.Invoke(this);
                _enemyDestroyedAction?.Invoke(this);
                IsAlive = false;
                ReleaseEnemy();
            }
        }

        public void SetDefaultProperties()
        {
            IsAlive = true;
            _spriteRenderer.color = _defaultColor;
            _enemyHealthController.Initialize(this, _gameSettings, KillEnemy, _onHealthChangedAction);

        }

        public void Move(Vector2 direction)
        {
            _rigidbody.velocity = direction * _enemySpeed;
        }

        public void StartMove()
        {
            _startMoving = true;
        }

        public void ReleaseEnemy()
        {
            ReleaseEnemyAction?.Invoke(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        private void Update()
        {
            if (_startMoving)
            {
                Move(Vector2.down);
            }
        }
    
        public void ChangeColor()
        {
            _spriteRenderer.color = Color.green;
        }
    }
}