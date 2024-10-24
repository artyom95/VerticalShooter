using System;
using Health.EnemyHealth;
using Interface;
using Settings;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, IMovable

    {
        [SerializeField] private Rigidbody _rigidbody;
        private EnemyHealthController _enemyHealthController;
        private float _enemySpeed;
        private bool _startMoving;
        private Action<Enemy> _enemyDestroyedAction;
        public bool IsAlive { get; private set; }
        public float Health;
        private Action<Enemy> _deleteEnemyFromList;
        private Action<Enemy> _enemyDeathAction;

        public void Start()
        {
            IsAlive = true;
        }

        public void Initialize(GameSettings gameSettings, Action<Enemy> enemyDestroyedAction,
            Action<Enemy> deleteEnemyFromList, Action<Enemy> enemyDeathAction, Action<Enemy, float> onHealthChangedAction)
        {
            _enemyDeathAction = enemyDeathAction;
            _deleteEnemyFromList = deleteEnemyFromList;
            _enemyDestroyedAction = enemyDestroyedAction;
            _enemyHealthController = new EnemyHealthController();
            _enemySpeed = gameSettings.EnemySpeed;
            _enemyHealthController.Initialize(this, gameSettings, KillEnemy, onHealthChangedAction);
            Health = _enemyHealthController.AmountHealth;
        }

        public void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.TryGetComponent<Bullet>(out var bullet))
            {
                Destroy(bullet.gameObject);
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
            if (gameObject != null)
            {
                _deleteEnemyFromList?.Invoke(this);
                _enemyDestroyedAction?.Invoke(this);
                IsAlive = false;
                Destroy(gameObject);
            }
        }

        public void Destroy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        public void Move(Vector2 direction)
        {
            _rigidbody.velocity = direction * _enemySpeed;
        }

        public void StartMove()
        {
            _startMoving = true;
        }

        private void Update()
        {
            if (_startMoving)
            {
                Move(Vector2.down);
            }
        }
    }
}