using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    /// <summary>
    /// should use objectPool instead of simple initializing bullet
    /// </summary>
    public class AttackExecutor : MonoBehaviour
    {
        private Bullet _bulletPrefab;
        private BulletFactory _bulletFactory;
        private Player _player;
        private GameSettings _gameSettings;
        private float _shootTimer;
        private ObjectPool<Bullet> _bulletPool;
        private float _bulletCoolDown;
        private float _nearestDistance;

        public void Initialize(Bullet bulletPrefab, Player player, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _bulletCoolDown = _gameSettings.BulletCoolDown;
            _nearestDistance = _gameSettings.NearDistanceToEnemy;
            _player = player;
            _bulletPrefab = bulletPrefab;
            _bulletPool = new ObjectPool<Bullet>(OnBulletCreate, OnTake,
                OnRelease, OnDestroyAction,
                true, 2,
                10);
            _bulletFactory = new BulletFactory();
        }

        public void Shoot(List<Enemy.Enemy> enemies)
        {
            if (!CheckShootTimer())
            {
                return;
            }

            var enemy = enemies.Find(enemy =>
                Vector3.Distance(_player.transform.position, enemy.transform.position) <= _nearestDistance);
            if (enemy.IsAlive)
            {
                enemy.ChangeColor();
                var bullet = CreateBullet();
                bullet.Move(enemy);
                _shootTimer = 0;
            }
        }

        private void Update()
        {
            _shootTimer += Time.deltaTime;
        }

        private bool CheckShootTimer()
        {
            if (_shootTimer >= _bulletCoolDown)
            {
                return true;
            }

            return false;
        }

        private Bullet CreateBullet()
        {
            var bullet = _bulletPool.Get();
            return bullet;
        }

        private void OnDestroyAction(Bullet bullet)
        {
            bullet.ReleaseBulletAction -= ReleaseBullet;
        }

        private void OnRelease(Bullet bullet)
        {
            var transformBullet = bullet.transform;

            bullet.gameObject.SetActive(false);
            transformBullet.rotation = Quaternion.identity;
            transformBullet.SetParent(_player.transform);
            transformBullet.position = _player.transform.position;
        }

        private void OnTake(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private Bullet OnBulletCreate()
        {
            var bullet = _bulletFactory.Create(_bulletPrefab, _player.transform);
            bullet.Initialize(_gameSettings);
            bullet.ReleaseBulletAction += ReleaseBullet;
            return bullet;
        }

        private void ReleaseBullet(Bullet bullet)
        {
            _bulletPool.Release(bullet);
        }
    }
}