using System;
using Settings;
using UI.Health;
using UI.StartScreen;
using UnityEngine;

namespace UI
{
    public class UIController
    {
        private readonly StartScreenController _startScreenController;
        private readonly PanelBehaviour _winPanel;
        private readonly PanelBehaviour _gameOverPanel;
        private readonly Action _unSubscribeAction;
        private readonly Action _initializeGameAction;
        private readonly PlayerHealthBarView _playerHealthBarView;
        private readonly EnemyHealthBarView _enemyHealthBarView;

        private readonly GameSettings _gameSettings;
        private readonly EnemyCounterView _enemyCounterView;
        private readonly RestartButton _restartButtonGameOverPanel;
        private readonly RestartButton _restartButtonWinPanel;
        private readonly Action _createControllers;
        private readonly Action _onRestartButtonPressed;
        private readonly HealthBar _healthBarPrefab;
        private readonly Transform _healthBarParent;
        private readonly HealthBarFactory _healthBarFactory;

        public UIController(StartScreenView startScreenView,
            StartButton startButton,
            RestartButton restartButtonGameOverPanel,
            RestartButton restartButtonWinPanel,
            GameSettings gameSettings,
            PanelBehaviour winPanel,
            PanelBehaviour gameOverPanel,
            EnemyCounterView enemyCounterView,
            HealthBar healthBarPrefab,
            Transform healthBarParent,
            Action createControllers,
            Action initializeGameAction,
            Action unSubscribeAction,
            Action onRestartButtonPressed)
        {
            _healthBarParent = healthBarParent;
            _healthBarPrefab = healthBarPrefab;
            _onRestartButtonPressed = onRestartButtonPressed;
            _createControllers = createControllers;
            _restartButtonWinPanel = restartButtonWinPanel;
            _restartButtonGameOverPanel = restartButtonGameOverPanel;

            _enemyCounterView = enemyCounterView;
            _gameSettings = gameSettings;
            _initializeGameAction = initializeGameAction;
            _unSubscribeAction = unSubscribeAction;
            _winPanel = winPanel;
            _gameOverPanel = gameOverPanel;
            _startScreenController = new StartScreenController(startScreenView,
                startButton, gameSettings, _initializeGameAction);
            _healthBarFactory = new HealthBarFactory();
            _playerHealthBarView = new PlayerHealthBarView();
            _enemyHealthBarView = new EnemyHealthBarView();
        }

        public void Initialize()
        {
            _playerHealthBarView.Initialize(_gameSettings, _healthBarPrefab, _healthBarFactory);
            _enemyHealthBarView.Initialize(_gameSettings, _healthBarPrefab, _healthBarFactory);
            _enemyCounterView.ShowAmountEnemy(_gameSettings.AmountEnemy);
            _gameOverPanel.Initialize(_unSubscribeAction, _createControllers, _initializeGameAction,
                _restartButtonGameOverPanel, _onRestartButtonPressed);
            _winPanel.Initialize(_unSubscribeAction, _createControllers, _initializeGameAction,
                _restartButtonWinPanel, _onRestartButtonPressed);
        }

        public void ShowWinPanel()
        {
            _winPanel.Show();
        }

        public void ShowGameOverPanel()
        {
            _gameOverPanel.Show();
        }

        public void ShowPlayerHealth(float playerHealth)
        {
            _playerHealthBarView.ShowHealth(playerHealth);
        }

        public void ShowEnemyHealth(Enemy.Enemy enemy, float enemyHealth)
        {
            _enemyHealthBarView.ShowHealth(enemy, enemyHealth);
        }

        public void ShowAmountEnemy(int amountEnemy)
        {
            _enemyCounterView.ShowAmountEnemy(amountEnemy);
        }

        public void OnPlayerSpawned(Player.Player player)
        {
            _playerHealthBarView.CreateHealthBar(_healthBarParent);
            _playerHealthBarView.SetRoot(player);
        }

        public void OnEnemySpawned(Enemy.Enemy enemy)
        {
            _enemyHealthBarView.CreateHealthBar(enemy, _healthBarParent);
            _enemyHealthBarView.SetRoot(enemy);
        }

        public void DestroyPlayerHealthBar()
        {
            _playerHealthBarView.DestroyHealthBar();
        }

        public void DestroyEnemyHealthBar(Enemy.Enemy enemy)
        {
            _enemyHealthBarView.DestroyHealthBar(enemy);
        }

        public void DestroyAliveEnemiesHealthBar()
        {
            _enemyHealthBarView.DestroyAllHealthBar();
        }
    }
}