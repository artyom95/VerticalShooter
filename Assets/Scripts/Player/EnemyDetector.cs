using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Player
{
    public class EnemyDetector
    {
        public event Action<List<Enemy.Enemy>> NearestEnemiesDetectedEvent;

        private List<Enemy.Enemy> _nearestEnemy;
        private readonly float _radiusDetection;
        private readonly float _nearDistanceToEnemy ;
        private Player _player;
        private readonly LayerMask _enemyLayer;

        public EnemyDetector(GameSettings gameSettings)
        {
            _nearestEnemy = new List<Enemy.Enemy>();
            _enemyLayer = gameSettings.EnemyLayer;
            _radiusDetection = gameSettings.RadiusDetection;
            _nearDistanceToEnemy = gameSettings.NearDistanceToEnemy;
        }

        public void DetectEnemies(Player player)
        {
           
            _player = player;
            var array = Physics.OverlapSphere(
                new Vector2(_player.transform.position.x, _player.transform.position.y),
                _radiusDetection, _enemyLayer);
            if (array.Length > 0)
            {
                GetNearestEnemies(array);
            }
        }

        public void DeleteEnemyFromNearestEnemyList(Enemy.Enemy enemy)
        {
            if (_nearestEnemy.Count == 0 || !_nearestEnemy.Contains(enemy))
            {
                return;
            }

            var enemyList = new List<Enemy.Enemy>();
            foreach (var memberEnemyList in _nearestEnemy)
            {
                if (!memberEnemyList.Equals(enemy))
                {
                    enemyList.Add(memberEnemyList);
                }
            }
            _nearestEnemy = enemyList;
        }
       
        private void GetNearestEnemies(Collider[] array)
        {
             _nearestEnemy.Clear();
            foreach (var collider2D in array)
            {
                if (Vector3.Distance(_player.transform.position, collider2D.transform.position) <= _nearDistanceToEnemy)
                {
                    collider2D.gameObject.TryGetComponent<Enemy.Enemy>(out var enemy);
                    _nearestEnemy.Add(enemy);
                }
            }

            if (_nearestEnemy.Count > 0)
            {
                NearestEnemiesDetectedEvent?.Invoke(_nearestEnemy);
            }
        }
        
    }
}