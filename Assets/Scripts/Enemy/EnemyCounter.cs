using System;
using System.Collections.Generic;
using Settings;

namespace Enemy
{
    public class EnemyCounter
    {
        private int _amountEnemy;
        private readonly List<Enemy> _enemiesList;

        private readonly Action<int> _amountEnemyChanged;
        private readonly Action _amountEnemyEndedAction;
        private readonly Action _amountAliveEndedAction;

        public EnemyCounter(GameSettings gameSettings,
            Action<int> amountEnemyChangedAction,
            Action amountEnemyEndedAction,
            Action amountAliveEndedAction)
        {
            _amountAliveEndedAction = amountAliveEndedAction;
            _amountEnemyEndedAction = amountEnemyEndedAction;
            _amountEnemyChanged = amountEnemyChangedAction;
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
            _amountEnemyChanged?.Invoke(_amountEnemy);

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
                _amountEnemyEndedAction?.Invoke();
            }
        }

        public void CheckAmountAliveEnemies()
        {
            if (_enemiesList.Count == 0)
            {
                _amountAliveEndedAction?.Invoke();
            }
        }
    }
}