using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "time-scaler", menuName = nameof(GGJ2025) + "/" + nameof(TimeScaler))]
    public class TimeScaler : ScriptableObject
    {
        public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
    }
}
