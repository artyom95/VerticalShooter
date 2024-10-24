using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PanelBehaviour : MonoBehaviour

    {
        private RestartButton _restartButton;

        public void Initialize(Action unSubscribeAction,
            Action createControllers, 
            Action initializeGameAction,
            RestartButton restartButton,
            Action onRestartButtonPressed)
        {
            _restartButton = restartButton;
            _restartButton.Initialize(unSubscribeAction,
                createControllers,
                initializeGameAction,
                onRestartButtonPressed);
            _restartButton.PressedRestartButton += Hide;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            UnSubscribe();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void UnSubscribe()
        {
            _restartButton.PressedRestartButton -= Hide;
        }
    }
}