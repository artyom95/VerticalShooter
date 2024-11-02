using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Player
{
    public class AttackExecutor : MonoBehaviour
    {
        private float _shootTimer;
        private float _bulletCoolDown;
        private float _nearestDistance;
        private GameSettings _gameSettings;
        private BulletController _bulletController;
        private Player _player;

        public void Initialize(Player player, GameSettings gameSettings,
            BulletController bulletController)
        {
            _bulletController = bulletController;
            _gameSettings = gameSettings;
            _bulletCoolDown = _gameSettings.BulletCoolDown;
            _nearestDistance = _gameSettings.NearDistanceToEnemy;
            _player = player;
            bulletController.Initialize(_player, gameSettings);
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
                var bullet = _bulletController.CreateBullet();
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
    }
}