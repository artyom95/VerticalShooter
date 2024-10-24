using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// should use objectPool instead of simple initializing bullet
    /// </summary>
    public class AttackExecutor : MonoBehaviour
    {
        private Bullet _bullet;
        private Bullet _bulletPrefab;
        private BulletSpawner _bulletSpawner;
        private Player _player;
        private GameSettings _gameSettings;
        private float _shootTimer;
        private readonly float _coolDown = 1f;

        public void Initialize(Bullet bulletPrefab, Player player, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _player = player;
            _bulletPrefab = bulletPrefab;
            _bulletSpawner = new BulletSpawner();
        }

        public void Shoot(List<Enemy.Enemy> enemies)
        {
            if (!CheckShootTimer())
            {
                return;
            }
            
            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive)
                {
                    _bullet = CreateBullet();
                    _bullet.Move(enemy.gameObject.transform);
                    _shootTimer = 0;
                    break;
                }
            }
        }

        private void Update()
        {
            _shootTimer += Time.deltaTime;
        }

        private bool CheckShootTimer()
        {
            if (_shootTimer >= _coolDown)
            {
                return true;
            }

            return false;
        }

        private Bullet CreateBullet()
        {
            var bullet = _bulletSpawner.CreateBullet(_bulletPrefab, _player.transform);
            bullet.Initialize(_gameSettings);
            return bullet;
        }
    }
}