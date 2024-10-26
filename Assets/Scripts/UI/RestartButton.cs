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
        private Action _createControllers;
        private Action _onRestartButtonPressed;

        public void Initialize(Action unSubscribeAction,
            Action createControllers,
            Action initializeGameAction,
            Action onRestartButtonPressed)
        {
            UnSubscribe();
            _onRestartButtonPressed = onRestartButtonPressed;
            _createControllers = createControllers;
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
            _onRestartButtonPressed = null;
            _createControllers = null;
            _initializeGameAction = null;
            _unSubscribeAction = null;
        }

        private void OnClick()
        {
            _unSubscribeAction?.Invoke();
            _onRestartButtonPressed?.Invoke();
            _createControllers?.Invoke();
            _initializeGameAction?.Invoke();
            PressedRestartButton?.Invoke();
        }
    }
}