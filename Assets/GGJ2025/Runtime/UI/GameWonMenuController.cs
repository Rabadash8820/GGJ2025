using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GGJ2025
{
    public class GameWonMenuController : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public UIDocument UIDocument;

        public string GrpGameWonMenuName = "game-won-menu";
        public string BtnPlayAgainName = "btn-play-again";
        public string BtnMainMenuName = "btn-main-menu";
        public string BtnQuitName = "btn-quit";

        public UnityEvent PlayAgainSelected = new();
        public UnityEvent MainMenuSelected = new();
        public UnityEvent QuitSelected = new();

        public UnityEvent PlayAgainHovered = new();
        public UnityEvent MainMenuHovered = new();
        public UnityEvent QuitHovered = new();

        private void Awake()
        {
            setupButtons(UIDocument.rootVisualElement);
        }

        private void setupButtons(VisualElement root)
        {
            VisualElement grpGameWonMenu = UIDocument.rootVisualElement.Query<VisualElement>(GrpGameWonMenuName).First();
            Button btnPlayAgain = root.Query<Button>(BtnPlayAgainName).First();
            Button btnMainMenu = root.Query<Button>(BtnMainMenuName).First();
            Button btnQuit = root.Query<Button>(BtnQuitName).First();

            btnPlayAgain.clicked += () => {
                Debug.Log("Play Again button clicked", context: this);
                grpGameWonMenu.style.display = DisplayStyle.Flex;
            };
            btnMainMenu.clicked += () => {
                Debug.Log("Main Menu button clicked", context: this);
                MainMenuSelected.Invoke();
            };
            btnQuit.clicked += () => {
                Debug.Log("Quit button clicked", context: this);
                QuitSelected.Invoke();
            };

            btnPlayAgain.RegisterCallback<MouseEnterEvent>(e => PlayAgainHovered.Invoke());
            btnMainMenu.RegisterCallback<MouseEnterEvent>(e => MainMenuHovered.Invoke());
            btnQuit.RegisterCallback<MouseEnterEvent>(e => { QuitHovered.Invoke(); });
        }
    }
}
