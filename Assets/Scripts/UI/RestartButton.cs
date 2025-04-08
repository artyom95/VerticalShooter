using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RestartButton : MonoBehaviour

    {
        public event Action PressedRestartButton;

        [SerializeField] private Button _restartButton;
        private Action _unSubscribeAction;
        private Action _initializeGameAction;
        private Action _createControllersAction;
        private Action _onRestartButtonPressedAction;

        public void Initialize(Action unSubscribeAction,
            Action createControllers,
            Action initializeGameAction,
            Action onRestartButtonPressed)
        {
            UnSubscribe();
            _onRestartButtonPressedAction = onRestartButtonPressed;
            _createControllersAction = createControllers;
            _initializeGameAction = initializeGameAction;
            _unSubscribeAction = unSubscribeAction;
            _restartButton.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void UnSubscribe()
        {
            _restartButton.onClick.RemoveListener(OnClick);
            _onRestartButtonPressedAction = null;
            _createControllersAction = null;
            _initializeGameAction = null;
            _unSubscribeAction = null;
        }

        private void OnClick()
        {
            _unSubscribeAction?.Invoke();
            _onRestartButtonPressedAction?.Invoke();
            _createControllersAction?.Invoke();
            _initializeGameAction?.Invoke();
            PressedRestartButton?.Invoke();
        }
    }
}