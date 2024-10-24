using Unity.VisualScripting;
using UnityEngine;

namespace UI.Health
{
    public class HealthBarPositionController : MonoBehaviour
    {
        [SerializeField] private RectTransform _uiElement;
        [SerializeField] private Vector3 _offset;
       
        private Transform _playerRoot;
        private Object _healthBarRoot;

        public void SetPlayerRoot(GameObject root)
        {
            _healthBarRoot = root;
        }

        private void Update()
        {
            if (_healthBarRoot == null)
            {
                return;
            }
            var pointInScreenSpace = RectTransformUtility.WorldToScreenPoint(Camera.main,
                _healthBarRoot.GameObject().transform.position + _offset);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform,
                pointInScreenSpace,
                null, out var localPoint);
            _uiElement.anchoredPosition = localPoint;
        }
    }
}