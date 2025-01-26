using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GGJ2025.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public UIDocument UIDocument;

        public string BtnPlayName = "btn-play";
        public string BtnCreditsName = "btn-credits";
        public string BtnQuitName = "btn-quit";

        public UnityEvent PlaySelected = new();
        public UnityEvent CreditsSelected = new();
        public UnityEvent QuitSelected = new();

        public UnityEvent PlayHovered = new();
        public UnityEvent CreditsHovered = new();
        public UnityEvent QuitHovered = new();

        private void Awake() => setupButtons(UIDocument.rootVisualElement);

        private void setupButtons(VisualElement root)
        {
            Button btnPlay = root.Query<Button>(BtnPlayName).First();
            Button btnCredits = root.Query<Button>(BtnCreditsName).First();
            Button btnQuit = root.Query<Button>(BtnQuitName).First();

            btnPlay.clicked += () => {
                Debug.Log("Play button clicked", context: this);
                PlaySelected.Invoke();
            };
            btnCredits.clicked += () => {
                Debug.Log("Credits button clicked", context: this);
                CreditsSelected.Invoke();
            };
            btnQuit.clicked += () => {
                Debug.Log("Quit button clicked", context: this);
                QuitSelected.Invoke();
            };

            btnPlay.RegisterCallback<MouseEnterEvent>(e => PlayHovered.Invoke());
            btnCredits.RegisterCallback<MouseEnterEvent>(e => CreditsHovered.Invoke());
            btnQuit.RegisterCallback<MouseEnterEvent>(e => { QuitHovered.Invoke(); });
        }
    }
}
