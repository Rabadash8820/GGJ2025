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

        public string GrpMainMenuName = "grp-main-menu";
        public string BtnPlayName = "btn-play";
        public string BtnCreditsName = "btn-credits";
        public string BtnQuitName = "btn-quit";

        public string GrpCreditsName = "grp-credits";
        public string BtnCreditsBackName = "btn-credits-back";

        public UnityEvent PlaySelected = new();
        public UnityEvent CreditsSelected = new();
        public UnityEvent QuitSelected = new();
        public UnityEvent CreditsBackSelected = new();

        public UnityEvent PlayHovered = new();
        public UnityEvent CreditsHovered = new();
        public UnityEvent QuitHovered = new();
        public UnityEvent CreditsBackHovered = new();

        private void Awake() => setupButtons(UIDocument.rootVisualElement);

        private void setupButtons(VisualElement root)
        {
            var grpMainMenu = root.Query<VisualElement>(GrpMainMenuName).First();
            Button btnPlay = root.Query<Button>(BtnPlayName).First();
            Button btnCredits = root.Query<Button>(BtnCreditsName).First();
            Button btnQuit = root.Query<Button>(BtnQuitName).First();

            var grpCredits = root.Query<VisualElement>(GrpCreditsName).First();
            Button btnCreditsBack = root.Query<Button>(BtnCreditsBackName).First();

            btnCredits.clicked += () => {
                grpMainMenu.style.display = DisplayStyle.None;
                grpCredits.style.display = DisplayStyle.Flex;
            };
            btnCreditsBack.clicked += () => {
                grpMainMenu.style.display = DisplayStyle.Flex;
                grpCredits.style.display = DisplayStyle.None;
            };

            registerButton(btnPlay, PlaySelected, PlayHovered);
            registerButton(btnCredits, CreditsSelected, CreditsHovered);
            registerButton(btnQuit, QuitSelected, QuitHovered);

            registerButton(btnCreditsBack, CreditsBackSelected, CreditsBackHovered);
        }

        private void registerButton(Button button, UnityEvent selectEvent, UnityEvent hoverEvent)
        {
            button.clicked += () => {
                Debug.Log($"Button '{button.name}' selected", context: this);
                selectEvent.Invoke();
            };
            button.RegisterCallback<MouseEnterEvent>(e => hoverEvent.Invoke());
        }
    }
}
