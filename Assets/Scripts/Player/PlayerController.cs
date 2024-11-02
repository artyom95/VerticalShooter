using System;
using Settings;
using UnityEngine;

namespace Player
{
    public class PlayerController : IDisposable
    {
        public event Action<Player> PlayerSpawned;

        private Player _player;
        private readonly PlayerMover _playerMover;
        private readonly EnemyDetector _enemyDetector;
        private readonly InputHandler _inputHandler;
        private readonly GameSettings _gameSettings;
        private readonly AttackExecutor _attackExecutor;
        private readonly Player _playerPrefab;
        private readonly Transform _playerSpawnTransform;

        public PlayerController(InputHandler inputHandler,
            Player playerPrefab, Transform playerSpawnTransform,
            PlayerMover playerMover, GameSettings gameSettings,
            Bullet bulletPrefab, AttackExecutor attackExecutor)
        {
            _playerSpawnTransform = playerSpawnTransform;
            _playerPrefab = playerPrefab;
            _attackExecutor = attackExecutor;
            _gameSettings = gameSettings;
            _inputHandler = inputHandler;
            _playerMover = playerMover;
            _enemyDetector = new EnemyDetector(_gameSettings);
        }


        public void Initialize(BulletController bulletController)
        {
            CreatePlayer(_playerPrefab, _playerSpawnTransform);
            _player.Initialize(_enemyDetector, _gameSettings);
            _inputHandler.StartListening();
            _playerMover.Initialize(_player, _gameSettings);
            _attackExecutor.Initialize(_player, _gameSettings,
                bulletController);
            Subscribe();
        }

        public void OnEnemyPrefabDestroyed(Enemy.Enemy enemy)
        {
            _enemyDetector.DeleteEnemyFromList(enemy);
        }

        public void KillPlayer()
        {
            if (_player == null)
            {
                return;
            }

            _player.Destroy();
            _player = null;
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        public void UnSubscribe()
        {
            _inputHandler.Move -= _playerMover.Move;
            _enemyDetector.NearestEnemiesDetectedEvent -= _attackExecutor.Shoot;
        }

        private void CreatePlayer(Player playerPrefab, Transform playerSpawnTransform)
        {
            var playerSpawner = new PlayerFactory();
            _player = playerSpawner.Create(playerPrefab, playerSpawnTransform);
            PlayerSpawned?.Invoke(_player);
        }

        private void Subscribe()
        {
            _inputHandler.Move += _playerMover.Move;
            _enemyDetector.NearestEnemiesDetectedEvent += _attackExecutor.Shoot;
        }
    }
}