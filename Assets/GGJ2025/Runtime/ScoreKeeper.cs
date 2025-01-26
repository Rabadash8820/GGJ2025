using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class ScoreKeeper : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public PersonHitter PersonHitter;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public BubbleCollector BubbleCollector;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public HudController HudController;

        [Min(0)]
        public int ScoreFactor = 5;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform PeopleParent;

        public UnityEvent AllPeopleHit = new();

        [ShowInInspector, ReadOnly] public int TotalPeopleCount { get; private set; }
        [ShowInInspector, ReadOnly] public int PeopleHitCount { get; private set; }
        [ShowInInspector, ReadOnly] public int Score { get; private set; }

        private void Awake()
        {
            TotalPeopleCount = PeopleParent.GetComponentsInChildren<PersonHead>().Length;

            PersonHitter.PersonHit.AddListener(() => {
                int pointsEarned = ScoreFactor * Mathf.CeilToInt(BubbleCollector.CurrentRadius);
                Score += pointsEarned;
                ++PeopleHitCount;

                Debug.Log($"Hit person {PeopleHitCount} / {TotalPeopleCount}, earning {pointsEarned} points with bubble radius {BubbleCollector.CurrentRadius}", context: this);
                HudController.UpdateScore(PeopleHitCount, Score);

                if (PeopleHitCount == TotalPeopleCount) {
                    Debug.Log("All people hit!", context: this);
                    AllPeopleHit.Invoke();
                }
            });
        }
    }
}
