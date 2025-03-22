using System;
using Settings;

namespace UI.StartScreen
{
    public class StartScreenController : IDisposable
    {
        private readonly StartScreenModel _startScreenModel;
        private readonly StartScreenView _startScreenView;
        private readonly StartButton _startButton;
        private readonly GameSettings _gameSettings;
        private readonly Action _initializeAction;

        public StartScreenController(StartScreenView startScreenView, StartButton startButton,
            GameSettings gameSettings, Action initializeAction)
        {
            _startScreenView = startScreenView;
            _startButton = startButton;
            _initializeAction = initializeAction;
            _gameSettings = gameSettings;
            _startScreenModel = new StartScreenModel();
            Initialize();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        private void Initialize()
        {
            Subscribe();
            _startButton.Initialize(_startScreenModel.BeginCountDown);
            _startScreenView.Initialize(_gameSettings);
            _startScreenModel.Initialize(_gameSettings);
        }

        private void Subscribe()
        {
            _startScreenModel.SetRemainingTime += _startScreenView.ShowRemainingTimeToStart;
            _startScreenModel.StopCountDown += OnStopCountDown;
            _startButton.PressedStartButton += _startScreenView.Enable;
        }

        private void OnStopCountDown()
        {
            _initializeAction?.Invoke();
            _startScreenView.Disable();
        }

        private void UnSubscribe()
        {
            _startScreenModel.SetRemainingTime -= _startScreenView.ShowRemainingTimeToStart;
            _startScreenModel.StopCountDown -= _startScreenView.Disable;
            _startScreenModel.StopCountDown -= OnStopCountDown;
            _startButton.PressedStartButton -= _startScreenView.Enable;
        }
    }
}