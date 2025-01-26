using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GGJ2025
{
    public class PauseMenuController : MonoBehaviour
    {
        private InputSystemActions _inputSystemActions;
        private VisualElement _grpPauseMenu;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public UIDocument UIDocument;

        public string GrpPauseMenuName = "pause-menu";
        public string BtnResumeName = "btn-resume";
        public string BtnMainMenuName = "btn-main-menu";
        public string BtnQuitName = "btn-quit";

        public UnityEvent ResumeSelected = new();
        public UnityEvent MainMenuSelected = new();
        public UnityEvent QuitSelected = new();

        public UnityEvent ResumeHovered = new();
        public UnityEvent MainMenuHovered = new();
        public UnityEvent QuitHovered = new();
        [Space]
        public UnityEvent Paused = new();
        public UnityEvent Resumed = new();

        public bool IsPaused { get; private set; }

        private void Awake()
        {
            _grpPauseMenu = UIDocument.rootVisualElement.Query<VisualElement>(GrpPauseMenuName).First();

            setupButtons(_grpPauseMenu);

            _inputSystemActions = new InputSystemActions();
            _inputSystemActions.Player.Pause.performed += _ => SetPaused(!IsPaused);
            _inputSystemActions.Player.Enable();
        }

        private void OnDestroy() => _inputSystemActions.Dispose();

        public void TogglePaused() => SetPaused(!IsPaused);
        public void SetPaused(bool isPaused)
        {
            if (isPaused == IsPaused)
                return;

            Debug.Log(isPaused ? "Pausing..." : "Resuming...", context: this);
            _grpPauseMenu.style.display = isPaused ? DisplayStyle.Flex : DisplayStyle.None;
            IsPaused = isPaused;
            (IsPaused ? Paused : Resumed).Invoke();
        }

        private void setupButtons(VisualElement root)
        {
            Button btnResume = root.Query<Button>(BtnResumeName).First();
            Button btnMainMenu = root.Query<Button>(BtnMainMenuName).First();
            Button btnQuit = root.Query<Button>(BtnQuitName).First();

            btnResume.clicked += () => {
                Debug.Log("Resume button clicked", context: this);
                SetPaused(!IsPaused);
            };
            btnMainMenu.clicked += () => {
                Debug.Log("Main Menu button clicked", context: this);
                MainMenuSelected.Invoke();
            };
            btnQuit.clicked += () => {
                Debug.Log("Quit button clicked", context: this);
                QuitSelected.Invoke();
            };

            btnResume.RegisterCallback<MouseEnterEvent>(e => ResumeHovered.Invoke());
            btnMainMenu.RegisterCallback<MouseEnterEvent>(e => MainMenuHovered.Invoke());
            btnQuit.RegisterCallback<MouseEnterEvent>(e => { QuitHovered.Invoke(); });
        }
    }
}
