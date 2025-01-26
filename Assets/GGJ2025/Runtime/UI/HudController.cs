using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ2025
{
    public class HudController : MonoBehaviour
    {
        private TextElement _lblScore;
        private TextElement _lblPeopleHit;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public UIDocument UIDocument;

        public string LblScoreName = "lbl-score";
        public string LblPeopleHitName = "lbl-people-hit";
        public string ScoreFormatString = "Score: {0}";
        public string PeopleHitFormatString = "People hit: {0}";

        private void Awake()
        {
            VisualElement root = UIDocument.rootVisualElement;
            _lblScore = root.Query<TextElement>(LblScoreName).First();
            _lblPeopleHit = root.Query<TextElement>(LblPeopleHitName).First();
        }

        public void UpdateScore(int peopleHit, int score)
        {
            _lblScore.text = string.Format(ScoreFormatString, score);
            _lblPeopleHit.text = string.Format(PeopleHitFormatString, peopleHit);
        }
    }
}
