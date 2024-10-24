using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TimerSettings", menuName = "ScriptableObjects/TimerSettings", order = 1)]
    public class TimerSettings : ScriptableObject

    {
        [field: SerializeField] public float CountdownTime;
        [field: SerializeField] public float ScaleX;
        [field: SerializeField] public float ScaleY;
        [field: SerializeField] public float DurationChangeScale;
        [field: SerializeField] public int Delay;
    }
}