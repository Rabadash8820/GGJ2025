using Sirenix.OdinInspector;
using UnityEngine;

namespace GGJ2025
{
    public class ScoreKeeper : MonoBehaviour
    {
        public int Score { get; private set; }

        [Min(0)]
        public int ScoreFactor = 5;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public PersonHitter PersonHitter;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public BubbleCollector BubbleCollector;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public HudController HudController;

        private void Awake() => PersonHitter.PersonHit.AddListener(() => {
            int pointsEarned = ScoreFactor * (int)BubbleCollector.CurrentRadius;
            Debug.Log($"Earned {pointsEarned} from bubble radius {BubbleCollector.CurrentRadius}");
            Score += pointsEarned;
            HudController.UpdateScore(Score);
        });
    }
}
