using Interfaces;
using Settings;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour, IMovable
    {
        private Player _player;
        private float _speed;
        private bool _isMoving;
        private Vector2 _direction;
        private float _moveTimer;
        private float _moveDuration;

        public void Initialize(Player player, GameSettings gameSettings)
        {
            _player = player;
            _speed = gameSettings.PlayerSpeed;
            _moveDuration = gameSettings.PlayerMoveDuration;
        }

        public void Move(Vector2 direction)
        {
            if (!_isMoving)
            {
                _isMoving = true;
                _direction = direction.normalized;

                _player.RigidBody.AddForce(_direction * _speed, ForceMode.Impulse);
                _player.RigidBody.angularVelocity = Vector3.zero;
                
                _moveTimer = 0;
            }
        }

        public void FixedUpdate()
        {
            if (_isMoving)
            {
                _moveTimer += Time.fixedDeltaTime;

                if (_moveTimer >= _moveDuration && _player.RigidBody != null)
                {
                    _isMoving = false;
                    _player.RigidBody.velocity = Vector2.zero;
                }
            }
        }
    }
}