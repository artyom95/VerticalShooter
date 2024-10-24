using System;
using DG.Tweening;
using Settings;
using TMPro;
using UnityEngine;

namespace UI.StartScreen
{
    public class StartScreenView : MonoBehaviour

    {
        [SerializeField] private TextMeshProUGUI _numberText;

        private float _scaleX;
        private float _scaleY;
        private float _durationChangeScale;


        public void Initialize(GameSettings gameSettings)
        {
            _scaleX = gameSettings.TimerSettings.ScaleX;
            _scaleY = gameSettings.TimerSettings.ScaleY;
            _durationChangeScale = gameSettings.TimerSettings.DurationChangeScale;
        }

        public void Enable()
        {
            _numberText.gameObject.SetActive(true);
        }

        public void ShowRemainingTimeToStart(float amountTime)
        {
            _numberText.text = Convert.ToString(amountTime);
            var sequence = DOTween.Sequence();
            var currentScale = _numberText.gameObject.transform.localScale;
            var nextScale = new Vector3(currentScale.x * _scaleX, currentScale.y * _scaleY, currentScale.z);
            sequence.Append(_numberText.gameObject.transform.DOScale(nextScale, _durationChangeScale));
            sequence.AppendCallback(() => _numberText.gameObject.transform.DOScale(currentScale, _durationChangeScale));
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}