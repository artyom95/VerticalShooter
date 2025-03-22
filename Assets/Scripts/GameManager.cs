using System.Collections.Generic;
using Enemy;
using Health.PlayerHealth;
using Player;
using Settings;
using UI;
using UI.Health;
using UI.StartScreen;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StartScreenView _startScreenView;
    [SerializeField] private StartButton _startButton;
    [SerializeField] private Player.Player _playerPrefab;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Transform _playerSpawnTransform;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private AttackExecutor _attackExecutor;
    [SerializeField] private Enemy.Enemy _enemyPrefab;
    [SerializeField] private List<Transform> _enemySpawnPoints;
    [SerializeField] private EnemySpawnTimer _enemySpawnTimer;
    [SerializeField] private FinishLineBehaviour _finishLineBehaviour;
    [SerializeField] private EnemyCounterView _enemyCounterView;
    [SerializeField] private RestartButton _restartButtonGameOverPanel;
    [SerializeField] private RestartButton _restartButtonWinPanel;
    [SerializeField] private HealthBar _healthBarPrefab;
    [SerializeField] private Transform _healthBarParent;
    [SerializeField] private PanelBehaviour _winPanel;
    [SerializeField] private PanelBehaviour _gameOverPanel;

    private PlayerHealthController _playerHealthController;
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private UIController _uiController;
    private BulletController _bulletController;

    public void Awake()
    {
        CreateControllers();
    }

    private void CreateControllers()
    {
        _uiController = new UIController(_startScreenView,
            _startButton, _restartButtonGameOverPanel,
            _restartButtonWinPanel, _gameSettings,
            _winPanel, _gameOverPanel,
            _enemyCounterView, _healthBarPrefab,
            _healthBarParent,
            CreateControllers, InitializeGame,
            UnSubscribe, OnRestartButtonPressed);

        _playerHealthController = new PlayerHealthController(_gameSettings);

        _playerController = new PlayerController(_inputHandler, _playerPrefab,
            _playerSpawnTransform, _playerMover,
            _gameSettings, _attackExecutor);

        _enemyController = new EnemyController(_gameSettings, _enemyPrefab,
            _enemySpawnPoints, _enemySpawnTimer);
        _bulletController = new BulletController(_bulletPrefab);
    }

    private void InitializeGame()
    {
        Subscribe();
        _enemyController.Initialize(_playerController.OnEnemyPrefabDestroyed);
        _uiController.Initialize();
        _playerController.Initialize(_bulletController);
    }

    private void Subscribe()
    {
        _finishLineBehaviour.EnemyDestroyedAction += _playerHealthController.DecreaseHealth;
        _finishLineBehaviour.EnemyDestroyed += _uiController.DestroyEnemyHealthBar;
        _playerHealthController.PlayerChangedEvent += _uiController.ShowPlayerHealth;
        _playerHealthController.PlayerHealthEndedEvent += OnPlayerHealthEnded;
        _enemyController.AmountEnemyChangedAction += _uiController.ShowAmountEnemy;
        _enemyController.AmountEnemyEndedAction += OnAmountEnemyEndedAction;
        _enemyController.EnemySpawned += _uiController.OnEnemySpawned;
        _enemyController.EnemyDied += _uiController.DestroyEnemyHealthBar;
        _enemyController.KillAllEnemies += _uiController.DestroyAliveEnemiesHealthBar;
        _enemyController.HealthChangedAction += _uiController.ShowEnemyHealth;
        _playerController.PlayerSpawned += _uiController.OnPlayerSpawned;
    }

    private void OnAmountEnemyEndedAction()
    {
        HandleGameEnd(true);
    }

    private void OnPlayerHealthEnded()
    {
        HandleGameEnd(false);
    }

    private void HandleGameEnd(bool isWin)
    {
        _enemyController.KillAliveEnemies();
        _uiController.DestroyPlayerHealthBar();
        _playerController.KillPlayer();

        if (isWin)
        {
            _uiController.ShowWinPanel();
        }
        else
        {
            _uiController.ShowGameOverPanel();
        }
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void UnSubscribe()
    {
        _playerController.UnSubscribe();
        _enemyController.UnSubscribe();
        _finishLineBehaviour.EnemyDestroyedAction -= _playerHealthController.DecreaseHealth;
        _finishLineBehaviour.EnemyDestroyed -= _uiController.DestroyEnemyHealthBar;
        _playerHealthController.PlayerHealthEndedEvent -= OnPlayerHealthEnded;
        _enemyController.AmountEnemyChangedAction -= _uiController.ShowAmountEnemy;
        _enemyController.AmountEnemyEndedAction -= OnAmountEnemyEndedAction;
        _enemyController.EnemySpawned -= _uiController.OnEnemySpawned;
        _enemyController.EnemyDied -= _uiController.DestroyEnemyHealthBar;
        _enemyController.KillAllEnemies -= _uiController.DestroyAliveEnemiesHealthBar;
        _enemyController.HealthChangedAction -= _uiController.ShowEnemyHealth;
        _playerHealthController.PlayerChangedEvent -= _uiController.ShowPlayerHealth;
        _playerController.PlayerSpawned -= _uiController.OnPlayerSpawned;
    }

    private void OnRestartButtonPressed()
    {
        _uiController = null;
        _playerHealthController = null;
        _playerController = null;
        _enemyController = null;
    }
}