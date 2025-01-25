using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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

    private void Awake() => setupButtons(UIDocument.rootVisualElement);

    private void setupButtons(VisualElement root)
    {
        Button btnPlayName = root.Query<Button>(BtnPlayName).First();
        Button btnCreditsName = root.Query<Button>(BtnCreditsName).First();
        Button btnQuitName = root.Query<Button>(BtnQuitName).First();

        btnPlayName.clicked += () => {
            Debug.Log("Play button clicked");
            PlaySelected.Invoke();
        };
        btnCreditsName.clicked += () => {
            Debug.Log("Credits button clicked");
            CreditsSelected.Invoke();
        };
        btnQuitName.clicked += () => {
            Debug.Log("Quit button clicked");
            QuitSelected.Invoke();
        };
    }
}
