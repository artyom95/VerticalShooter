using System;
using System.Collections.Generic;
using Settings;

namespace Enemy
{
    public class EnemyCounter
    {
        private int _amountEnemy;
        private readonly List<Enemy> _enemiesList;

        private readonly Action<int> _amountEnemiesChanged;
        private readonly Action _amountEnemiesEndedAction;
        private readonly Action _amountAliveEnemiesEndedAction;

        public EnemyCounter(GameSettings gameSettings,
            Action<int> amountEnemiesChangedAction,
            Action amountEnemiesEndedAction,
            Action amountAliveEnemiesEndedAction)
        {
            _amountAliveEnemiesEndedAction = amountAliveEnemiesEndedAction;
            _amountEnemiesEndedAction = amountEnemiesEndedAction;
            _amountEnemiesChanged = amountEnemiesChangedAction;
            _amountEnemy = gameSettings.AmountEnemy;
            _enemiesList = new List<Enemy>();
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemiesList.Add(enemy);
        }

        public void DeleteEnemy(Enemy enemy)
        {
            _enemiesList.Remove(enemy);
        }

        public void DecreaseAmountEnemy()
        {
            _amountEnemy -= 1;
            _amountEnemiesChanged?.Invoke(_amountEnemy);

            CheckAmountEnemy();
        }

        public void KillAllEnemies()
        {
            foreach (var enemy in _enemiesList)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    enemy.ReleaseEnemy();
                }
            }
        }

        private void CheckAmountEnemy()
        {
            if (_amountEnemy <= 0)
            {
                _amountEnemiesEndedAction?.Invoke();
            }
        }

        public void CheckAmountAliveEnemies()
        {
            if (_enemiesList.Count == 0)
            {
                _amountAliveEnemiesEndedAction?.Invoke();
            }
        }
    }
}