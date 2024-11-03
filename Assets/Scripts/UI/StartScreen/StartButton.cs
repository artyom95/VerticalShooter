using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StartScreen
{
    public class StartButton : MonoBehaviour
    {
        public event Action PressedStartButton;

        [SerializeField] private Button _startButton;

        private Action _onClickStartButton;

        public void Initialize(Action onClickStartButton)
        {
            _onClickStartButton = onClickStartButton;
            _startButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _onClickStartButton?.Invoke();
            gameObject.SetActive(false);
            PressedStartButton?.Invoke();
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnClick);
        }
    }
}