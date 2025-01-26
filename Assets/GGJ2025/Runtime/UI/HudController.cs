using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ2025
{
    public class HudController : MonoBehaviour
    {
        private TextElement _lblScore;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public UIDocument UIDocument;

        public string LblScoreName = "lbl-score";
        public string ScoreFormatString = "Score: {0}";

        private void Awake()
        {
            VisualElement root = UIDocument.rootVisualElement;
            _lblScore = root.Query<TextElement>(LblScoreName).First();
        }

        public void UpdateScore(float score) => _lblScore.text = string.Format(ScoreFormatString, score);
    }
}
