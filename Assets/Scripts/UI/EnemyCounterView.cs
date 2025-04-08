using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnemyCounterView : MonoBehaviour

    {
        [SerializeField] private TextMeshProUGUI _amountEnemy;

        public void ShowAmountEnemy(int amountEnemy)
        {
            _amountEnemy.text = Convert.ToString(amountEnemy);
        }
    }
}