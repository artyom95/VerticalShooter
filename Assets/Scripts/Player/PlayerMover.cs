using Interface;
using Settings;
using TMPro;
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
                _isMoving = true; // Устанавливаем флаг, что объект начал двигаться
                _direction = direction.normalized;

                // Применяем силу для старта
                _player.RigidBody.AddForce(_direction * _speed, ForceMode.Impulse);

                _moveTimer = 0; // Сбрасываем таймер
            }
        }

        public void FixedUpdate()
        {
            // Поддерживаем эффект только до истечения времени

            if (_isMoving)
            {
                _moveTimer += Time.fixedDeltaTime; // Увеличиваем таймер

                // Проверяем, превышает ли таймер максимальную длительность движения
                if (_moveTimer >= _moveDuration)
                {
                    _isMoving = false; // Остановка движения
                    _player.RigidBody.velocity = Vector2.zero; // Остановить объект
                }

                // Здесь можно дополнительно обрабатывать поведение объекта, пока он движется.
            }
        }
    }
}