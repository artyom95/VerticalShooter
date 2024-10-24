using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class EnemyDetector
    {
        public event Action<List<Enemy.Enemy>> NearestEnemiesDetectedEvent;

        //   private List<Enemy.Enemy> _enemies;
        private List<Enemy.Enemy> _nearestEnemy;
        private float _radiusDetection = 4f;
        private float _nearDistanceToEnemy = 3f;
        private Player _player;
        private LayerMask _enemyLayer;

        public EnemyDetector(LayerMask enemyLayer)
        {
            _nearestEnemy = new List<Enemy.Enemy>();
            _enemyLayer = enemyLayer;
        }

        public void DetectEnemies(Player player)
        {
            Debug.Log("DetectEnemies method was invoke");

            _player = player;
            var array = Physics.OverlapSphere(
                new Vector2(_player.transform.position.x, _player.transform.position.y),
                _radiusDetection, _enemyLayer);
            if (array.Length > 0)
            {
                GetNearestEnemies(array);
            }
        }

        public void DeleteEnemyFromList(Enemy.Enemy enemy)
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
        /// <summary>
        /// how to pull through  Physics2D.OverlapCircleAll method necessary script for me
        /// </summary>
        /// <param name="array"></param>
        private void GetNearestEnemies(Collider[] array)
        {
            Debug.Log("GetNearestEnemies method was created");
            
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